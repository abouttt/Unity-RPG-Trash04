using System;
using UnityEngine;

public interface IStackableItem : IItem
{
    public event Action StackChanged;

    int Count { get; }
    int MaxCount { get; }
}
