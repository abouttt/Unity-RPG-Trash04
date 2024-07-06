using UnityEngine;

public abstract class StackableItemData : ItemData
{
    [field: Header("Stackable Data")]

    [field: SerializeField]
    public int MaxCount { get; private set; } = 99;

    public StackableItemData(ItemType itemType)
        : base(itemType)
    { }

    public abstract Item CreateItem(int count);
}
