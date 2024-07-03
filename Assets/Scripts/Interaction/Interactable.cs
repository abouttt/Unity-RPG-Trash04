using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [field: SerializeField, ReadOnly]
    public bool IsDetected { get; set; }    // 감지 되었는지

    [field: SerializeField, ReadOnly]
    public bool IsInteracted { get; private set; }  // 상호작용 중인지

    [field: SerializeField]
    public string Message { get; protected set; }

    [field: SerializeField]
    public float LoadingTime { get; protected set; }    // 상호작용까지의 시간

    [field: SerializeField]
    public bool CanInteract { get; protected set; } = true;

    public virtual void Interact()
    {
        IsInteracted = true;
    }

    public virtual void Deinteract()
    {
        IsInteracted = false;
    }
}
