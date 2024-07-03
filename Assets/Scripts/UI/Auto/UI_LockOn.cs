using UnityEngine;

public class UI_LockOn : UI_Auto
{
    public Transform Target
    {
        get => _followTarget.Target;
        set => _followTarget.Target = value;
    }

    private UI_FollowWorldObject _followTarget;

    protected override void Init()
    {
        _followTarget = GetComponent<UI_FollowWorldObject>();
    }
}
