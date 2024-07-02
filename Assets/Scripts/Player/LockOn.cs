using System;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            IsLockOn = _target != null;
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
    public LayerMask TargetMask { get; set; }

    [field: SerializeField]
    public LayerMask ObstacleMask { get; set; }

    private Transform _target;

    public void FindTarget(Transform start, Predicate<Transform> otherCheckLogic = null)
    {
        float shortestAngle = Mathf.Infinity;
        Transform finalTarget = null;

        var targets = Physics.OverlapSphere(start.position, ViewRadius, TargetMask);
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

            if (Physics.Linecast(start.position, target.transform.position, ObstacleMask))
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

        if (obstacle && Physics.Linecast(start.position, _target.position, ObstacleMask))
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
