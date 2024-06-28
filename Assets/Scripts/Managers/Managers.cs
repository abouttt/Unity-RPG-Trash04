using System;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    public static GameManager Game => Instance._game;
    public static InputManager Input => Instance._input;
    public static PoolManager Pool => Instance._pool;
    public static ResourceManager Resource => Instance._resource;
    public static SceneManagerEx Scene => Instance._scene;
    public static SoundManager Sound => Instance._sound;
    public static UIManager UI => Instance._ui;

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

        _pool.Init();
        _resource.Init();
        _sound.Init();
        _ui.Init();
    }

    public static void Clear()
    {
        Input.Clear();
        Pool.Clear();
        Resource.Clear();
        Sound.Clear();
        UI.Clear();
        GC.Collect();
    }
}
