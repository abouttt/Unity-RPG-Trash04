using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [field: SerializeField, ReadOnly]
    public bool IsDetected { get; set; }    // ���� �Ǿ�����

    [field: SerializeField, ReadOnly]
    public bool IsInteracted { get; private set; }  // ��ȣ�ۿ� ������

    [field: SerializeField]
    public string InteractionName { get; private set; }

    [field: SerializeField]
    public string InteractionMessage { get; protected set; }

    [field: SerializeField]
    public Vector3 UIOffset { get; protected set; }

    [field: SerializeField]
    public float LoadingTime { get; protected set; }    // ��ȣ�ۿ������ �ð�

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

    // InteractionKeyGuidePos ��ġ �ð�ȭ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + UIOffset, 0.1f);
    }
}
