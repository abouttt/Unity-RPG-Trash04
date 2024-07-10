using System;
using UnityEngine;

public interface IStackable
{
    public event Action StackChanged;

    int Count { get; }
    int MaxCount { get; }
}
