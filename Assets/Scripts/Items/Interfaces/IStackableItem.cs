using UnityEngine;

public interface IStackableItem : IItem
{
    public StackableItemData StackableData { get; }
    public int Count { get; }
    public int MaxCount { get; }
}
