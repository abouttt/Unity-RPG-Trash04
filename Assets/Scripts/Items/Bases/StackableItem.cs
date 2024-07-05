using UnityEngine;

public abstract class StackableItem : Item, IStackableItem
{
    public StackableItemData StackableData { get; private set; }

    public int Count
    {
        get => _count;
        set => _count = Mathf.Clamp(_count, 0, MaxCount);
    }

    public int MaxCount => StackableData.MaxCount;
    public bool IsMax => Count >= MaxCount;
    public bool IsEmpty => Count <= 0;

    private int _count;

    public StackableItem(StackableItemData stackableData, int count)
        : base(stackableData)
    {
        StackableData = stackableData;
        Count = count;
    }

    public int AddCountAndGetExcess(int count)
    {
        int nextCount = Count + count;
        Count = count;
        return nextCount > MaxCount ? nextCount - MaxCount : 0;
    }
}
