using System;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    public static CooldownManager Cooldown => Instance._cooldown;
    public static GameManager Game => Instance._game;
    public static InputManager Input => Instance._input;
    public static PoolManager Pool => Instance._pool;
    public static ResourceManager Resource => Instance._resource;
    public static SceneManagerEx Scene => Instance._scene;
    public static SoundManager Sound => Instance._sound;
    public static UIManager UI => Instance._ui;

    private readonly CooldownManager _cooldown = new();
    private readonly GameManager _game = new();
    private readonly InputManager _input = new();
    private readonly PoolManager _pool = new();
    private readonly ResourceManager _resource = new();
    private readonly SceneManagerEx _scene = new();
    private readonly SoundManager _sound = new();
    private readonly UIManager _ui = new();

    protected override void Awake()
    {
        base.Awake();

        _input.Init();
        _pool.Init();
        _resource.Init();
        _sound.Init();
        _ui.Init();
    }

    private void LateUpdate()
    {
        Cooldown.UpdateCooldowns();
    }

    public static void Clear()
    {
        Cooldown.Clear();
        Pool.Clear();
        Resource.Clear();
        Sound.Clear();
        UI.Clear();
        GC.Collect();
    }
}
