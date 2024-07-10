using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public event Action<Interactable> TargetChanged;

    public Interactable Target { get; private set; }
    public bool Interact { get; set; }
    public float ProgressedLoadingTime { get; private set; }

    [field: SerializeField]
    public LayerMask TargetLayers { get; set; }

    [field: SerializeField]
    public LayerMask ObstacleLayers { get; set; }

    private bool _canInteract;
    private bool _isTargetRangeOut;

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        if (!Target.gameObject.activeSelf)
        {
            SetTarget(null);
            return;
        }

        if (Target.IsInteracted)
        {
            return;
        }

        if (_isTargetRangeOut)
        {
            SetTarget(null);
            return;
        }

        if (Interact)
        {
            if (_canInteract)
            {
                ProgressedLoadingTime += Time.deltaTime;
                if (ProgressedLoadingTime >= Target.LoadingTime)
                {
                    ProgressedLoadingTime = 0f;
                    Target.Interact();
                }
            }
        }
        else
        {
            ProgressedLoadingTime = 0f;
            _canInteract = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CheckLayers(TargetLayers))
        {
            return;
        }

        if (Target == null)
        {
            SetTarget(other.GetComponent<Interactable>());
        }
        else
        {
            if (Target.IsInteracted)
            {
                return;
            }

            if (Target.gameObject != other.gameObject)
            {
                float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
                float distanceToOther = Vector3.Distance(transform.position, other.transform.position);
                if (distanceToTarget <= distanceToOther)
                {
                    return;
                }

                if (Physics.Linecast(transform.position, other.transform.position, ObstacleLayers))
                {
                    return;
                }

                SetTarget(other.GetComponent<Interactable>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CheckLayers(TargetLayers))
        {
            return;
        }

        if (Target.gameObject != other.gameObject)
        {
            return;
        }

        if (Target.IsInteracted)
        {
            _isTargetRangeOut = true;
        }
        else
        {
            SetTarget(null);
        }
    }

    private void SetTarget(Interactable target)
    {
        if (Target == target)
        {
            return;
        }

        if (Target != null)
        {
            Target.IsDetected = false;
        }

        ProgressedLoadingTime = 0f;
        Target = target;
        _isTargetRangeOut = false;
        _canInteract = false;

        if (Target != null)
        {
            Target.IsDetected = true;
        }

        TargetChanged?.Invoke(Target);
    }
}
