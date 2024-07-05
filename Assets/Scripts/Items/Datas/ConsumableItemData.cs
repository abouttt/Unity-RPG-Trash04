using UnityEngine;

public abstract class ConsumableItemData : StackableItemData, ICooldownable
{
    [field: Header("Consumable Data")]

    [field: SerializeField]
    public int LimitLevel { get; private set; } = 1;

    [field: SerializeField]
    public int RequiredCount { get; private set; } = 1;

    [field: SerializeField]
    public float CoolTime { get; set; }

    [field: SerializeField]
    public float MaxCoolTime { get; private set; }

    public ConsumableItemData()
        : base(ItemType.Consumable)
    {
    }
}
