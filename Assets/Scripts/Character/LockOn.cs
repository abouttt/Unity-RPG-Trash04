using System;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public event Action<Transform> TargetChanged;

    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            IsLockOn = _target != null;
            TargetChanged?.Invoke(_target);
        }
    }

    public bool IsLockOn { get; private set; }

    [field: SerializeField]
    public float ViewRadius { get; set; }

    [field: SerializeField]
    public float MinViewAngle { get; set; }

    [field: SerializeField]
    public float MaxViewAngle { get; set; }

    [field: SerializeField]
    public LayerMask TargetLayers { get; set; }

    [field: SerializeField]
    public LayerMask ObstacleLayers { get; set; }

    private Transform _target;

    public void FindTarget(Transform start, Predicate<Transform> otherCheckLogic = null)
    {
        float shortestAngle = Mathf.Infinity;
        Transform finalTarget = null;

        var targets = Physics.OverlapSphere(start.position, ViewRadius, TargetLayers);
        foreach (var target in targets)
        {
            var directionToTarget = (target.transform.position - start.position).normalized;
            float viewAngle = Vector3.Angle(start.forward, directionToTarget);

            if (viewAngle < MinViewAngle || viewAngle > MaxViewAngle || viewAngle >= shortestAngle)
            {
                continue;
            }

            if (!IsPitchInRange(Quaternion.LookRotation(directionToTarget).eulerAngles.x))
            {
                continue;
            }

            if (Physics.Linecast(start.position, target.transform.position, ObstacleLayers))
            {
                continue;
            }

            if (otherCheckLogic == null || !otherCheckLogic(target.transform))
            {
                continue;
            }

            finalTarget = target.transform;
            shortestAngle = viewAngle;
        }

        Target = finalTarget;
    }

    public void TrackingTarget(Transform start, bool active = true, bool distance = true, bool obstacle = true, bool angle = true)
    {
        if (active && !_target.gameObject.activeInHierarchy)
        {
            Target = null;
            return;
        }

        if (distance && Vector3.Distance(start.position, _target.position) > ViewRadius)
        {
            Target = null;
            return;
        }

        if (obstacle && Physics.Linecast(start.position, _target.position, ObstacleLayers))
        {
            Target = null;
            return;
        }

        if (angle && !IsPitchInRange(start.eulerAngles.x))
        {
            Target = null;
        }
    }

    private bool IsPitchInRange(float pitch)
    {
        pitch = Util.ClampAngle(pitch, MinViewAngle, MaxViewAngle);
        if (pitch < MinViewAngle || pitch > MaxViewAngle)
        {
            return false;
        }

        return true;
    }
}
