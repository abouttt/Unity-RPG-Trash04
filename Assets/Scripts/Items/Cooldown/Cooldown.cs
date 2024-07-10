using System;
using UnityEngine;

[Serializable]
public class Cooldown
{
    public event Action CooldownStarted;

    public float MaxTime { get; private set; }
    public float RemainingTime { get; set; }

    public void Start()
    {
        RemainingTime = MaxTime;
        CooldownStarted?.Invoke();
    }

    public void Clear()
    {
        RemainingTime = 0f;
        CooldownStarted = null;
    }
}
