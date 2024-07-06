using UnityEngine;

public interface IStackableItem : IItem
{
    int Count { get; }
    int MaxCount { get; }
}
