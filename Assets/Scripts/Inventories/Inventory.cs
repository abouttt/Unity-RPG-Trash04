using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory
{
    [field: SerializeField]
    public int Capacity { get; private set; }

    [field: SerializeField, ReadOnly]
    public int Count { get; private set; }

    public IReadOnlyList<Item> Items => _items;

    private List<Item> _items;
    private readonly Dictionary<Item, int> _indexes = new();

    public void Init()
    {
        if (_items != null)
        {
            return;
        }

        var nullItems = Enumerable.Repeat<Item>(null, Capacity);
        _items = new(nullItems);
    }

    public bool SetItem(ItemData itemData, int index, int count)
    {
        if (itemData == null)
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

        var newItem = itemData is StackableItemData stackableItemData
            ? stackableItemData.CreateItem(count)
            : itemData.CreateItem();
        _items[index] = newItem;
        _indexes.Add(newItem, index);
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

    public T GetItem<T>(int index) where T : Item
    {
        if (!IsIndexInRange(index))
        {
            return null;
        }

        return _items[index] as T;
    }

    public int GetItemIndex(Item item)
    {
        if (_indexes.TryGetValue(item, out var index))
        {
            return index;
        }

        return -1;
    }

    public int FindSameItemIndex(ItemData itemData, int startIndex = 0)
    {
        if (itemData == null)
        {
            return -1;
        }

        if (!IsIndexInRange(startIndex))
        {
            return -1;
        }

        return _items.FindIndex(startIndex, item => item != null && item.Data.Equals(itemData));
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

        var nullItems = Enumerable.Repeat<Item>(null, capacity);
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
