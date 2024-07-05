using UnityEngine;

public interface ICooldownable
{
    public float CoolTime { get; }
    public float MaxCoolTime { get; }
}
