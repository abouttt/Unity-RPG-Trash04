using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public event Action<Interactable> TargetChanged;

    public Interactable Target
    {
        get => _target;
        private set
        {
            if (_target == value)
            {
                return;
            }

            if (_target != null)
            {
                _target.IsDetected = false;
            }

            ProgressedLoadingTime = 0f;
            _target = value;
            _isTargetRangeOut = false;
            _canInteract = false;

            if (_target != null)
            {
                _target.IsDetected = true;
            }

            TargetChanged?.Invoke(_target);
        }
    }

    public bool Interact { get; set; }
    public float ProgressedLoadingTime { get; private set; }

    [field: SerializeField]
    public LayerMask TargetLayers { get; set; }

    [field: SerializeField]
    public LayerMask ObstacleLayers { get; set; }

    private Interactable _target;
    private bool _canInteract;
    private bool _isTargetRangeOut;

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        if (!_target.gameObject.activeSelf)
        {
            Target = null;
            return;
        }

        if (_target.IsInteracted)
        {
            return;
        }

        if (_isTargetRangeOut)
        {
            Target = null;
            return;
        }

        if (Interact)
        {
            if (_canInteract)
            {
                ProgressedLoadingTime += Time.deltaTime;
                if (ProgressedLoadingTime >= _target.LoadingTime)
                {
                    ProgressedLoadingTime = 0;
                    _target.Interact();
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

        if (_target == null)
        {
            Target = other.GetComponent<Interactable>();
        }
        else
        {
            if (_target.IsInteracted)
            {
                return;
            }

            if (_target.gameObject != other.gameObject)
            {
                var distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);
                var distanceToOther = Vector3.Distance(transform.position, other.transform.position);
                if (distanceToTarget <= distanceToOther)
                {
                    return;
                }

                if (Physics.Linecast(transform.position, other.transform.position, ObstacleLayers))
                {
                    return;
                }

                Target = other.GetComponent<Interactable>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CheckLayers(TargetLayers))
        {
            return;
        }

        if (_target.gameObject != other.gameObject)
        {
            return;
        }

        if (_target.IsInteracted)
        {
            _isTargetRangeOut = true;
        }
        else
        {
            Target = null;
        }
    }
}
