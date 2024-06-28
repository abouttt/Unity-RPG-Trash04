using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SoundManager
{
    private Transform _root;
    private readonly List<AudioSource> _audioSources = new();

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject("Sound_Root").transform;
            Object.DontDestroyOnLoad(_root);

            foreach (var typeName in Enum.GetNames(typeof(SoundType)))
            {
                var go = new GameObject(typeName);
                _audioSources.Add(go.AddComponent<AudioSource>());
                go.transform.SetParent(_root);
            }

            _audioSources[(int)SoundType.BGM].loop = true;
        }
    }

    public void Play(SoundType soundType, string key)
    {
        var audioClip = Managers.Resource.Load<AudioClip>(key);
        Play(soundType, audioClip);
    }

    public void Play(SoundType soundType, AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return;
        }

        var audioSource = _audioSources[(int)soundType];

        if (soundType == SoundType.BGM)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Stop(SoundType soundType)
    {
        _audioSources[(int)soundType].Stop();
    }

    public AudioSource GetAudioSource(SoundType soundType)
    {
        return _audioSources[(int)soundType];
    }

    public void Clear()
    {
        foreach (var audioSource in _audioSources)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
}
