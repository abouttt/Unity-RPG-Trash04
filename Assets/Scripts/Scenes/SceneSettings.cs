using System;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "Settings/Scene Settings", fileName = "SceneSettings")]
public class SceneSettings : SingletonScriptableObject<SceneSettings>
{
    [Serializable]
    public class Settings
    {
        [field: SerializeField]
        public AddressableLabel[] ResourcesLoadLabels { get; private set; }

        [field: SerializeField]
        public bool ReloadSceneWhenNoResources { get; private set; }

        [field: SerializeField]
        public Sprite Background { get; private set; }

        [field: SerializeField]
        public AudioClip BGM { get; private set; }
    }

    public Settings this[SceneType sceneType] => _settings[sceneType];

    [SerializeField]
    private SerializedDictionary<SceneType, Settings> _settings;
}
