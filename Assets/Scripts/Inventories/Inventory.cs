using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory<T> where T : class
{
    [field: SerializeField]
    public int Capacity { get; private set; }

    [field: SerializeField, ReadOnly]
    public int Count { get; private set; }

    public IReadOnlyList<T> Items => _items;

    private List<T> _items;
    private readonly Dictionary<T, int> _indexes = new();

    public void Init(int capacity = -1)
    {
        if (_items != null)
        {
            return;
        }

        if (capacity > 0)
        {
            Capacity = capacity;
        }

        var nullItems = Enumerable.Repeat<T>(null, Capacity);
        _items = new(nullItems);
    }

    public bool SetItem(T item, int index, int count)
    {
        if (item == null)
        {
            return false;
        }

        if (!IsIndexInRange(index))
        {
            return false;
        }

        if (count <= 0)
        {
            return false;
        }

        if (_items[index] != null)
        {
            RemoveItem(index);
        }

        _items[index] = item;
        _indexes.Add(item, index);
        Count++;

        return true;
    }

    public bool RemoveItem(int index)
    {
        if (!IsIndexInRange(index))
        {
            return false;
        }

        if (_items[index] == null)
        {
            return false;
        }

        _indexes.Remove(_items[index]);
        _items[index] = null;
        Count--;

        return true;
    }

    public U GetItem<U>(int index) where U : class, T
    {
        if (!IsIndexInRange(index))
        {
            return null;
        }

        return _items[index] as U;
    }

    public int GetItemIndex(T item)
    {
        if (_indexes.TryGetValue(item, out var index))
        {
            return index;
        }

        return -1;
    }

    public int FindSameItemIndex(int startIndex, Predicate<T> logic)
    {
        if (!IsIndexInRange(startIndex))
        {
            return -1;
        }

        return _items.FindIndex(startIndex, logic);
    }

    public bool IsEmptyIndex(int index)
    {
        if (!IsIndexInRange(index))
        {
            return false;
        }

        return _items[index] == null;
    }

    public int FindEmptyIndex(int startIndex = 0)
    {
        if (!IsIndexInRange(startIndex))
        {
            return -1;
        }

        return _items.FindIndex(startIndex, item => item == null);
    }

    public void SwapItem(int indexA, int indexB)
    {
        if (!IsIndexInRange(indexA) || !IsIndexInRange(indexB))
        {
            return;
        }

        if (_items[indexA] != null)
        {
            _indexes[_items[indexA]] = indexB;
        }

        if (_items[indexB] != null)
        {
            _indexes[_items[indexB]] = indexA;
        }

        (_items[indexA], _items[indexB]) = (_items[indexB], _items[indexA]);
    }

    public void AddCapacity(int capacity)
    {
        if (capacity <= 0)
        {
            return;
        }

        var nullItems = Enumerable.Repeat<T>(null, capacity);
        _items.AddRange(nullItems);
    }

    public bool IsIndexInRange(int index)
    {
        return index >= 0 && index < _items.Count;
    }

    public void Clear()
    {
        Count = 0;
        _items.Clear();
        _indexes.Clear();
    }
}
