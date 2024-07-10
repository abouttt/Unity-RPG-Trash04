using UnityEngine;

public abstract class ConsumableItemData : StackableItemData, ILimitLevelableItemData, ICooldownable
{
    [field: Header("Consumable Data")]

    [field: SerializeField]
    public int LimitLevel { get; private set; } = 1;

    [field: SerializeField]
    public int RequiredCount { get; private set; } = 1;

    [field: SerializeField]
    public Cooldown Cooldown { get; private set; }

    public ConsumableItemData()
        : base(ItemType.Consumable)
    { }
}
