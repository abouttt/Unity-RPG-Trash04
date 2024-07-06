using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public int Capacity { get; private set; }
    public int Count { get; private set; }
    public IReadOnlyList<Item> Items => _items;

    public List<Item> _items;
    public Dictionary<Item, int> _indexes = new();

    public Inventory(int capacity)
    {
        Capacity = capacity;
        _items = Enumerable.Repeat<Item>(null, capacity).ToList();
    }

    public void SetItem(ItemData itemData, int index, int count)
    {
        if (itemData == null)
        {
            return;
        }

        if (count <= 0)
        {
            return;
        }

        if (_items[index] != null)
        {
            RemoveItem(index);
        }

        var newItem = itemData is StackableItemData stackableData ? stackableData.CreateItem(count) : itemData.CreateItem();
        _items[index] = newItem;
        _indexes.Add(newItem, index);
        Count++;
    }

    public void RemoveItem(int index)
    {
        if (_items[index] == null)
        {
            return;
        }

        _indexes.Remove(_items[index]);
        _items[index] = null;
        Count--;
    }

    public T GetItem<T>(int index) where T : Item
    {
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

    public bool IsEmptyIndex(int index)
    {
        return _items[index] == null;
    }

    public int FindEmptyIndex(int startIndex = 0)
    {
        return _items.FindIndex(startIndex, item => item == null);
    }

    public void SwapItem(int index1, int index2)
    {
        if (_items[index1] != null)
        {
            _indexes[_items[index1]] = index2;
        }

        if (_items[index2] != null)
        {
            _indexes[_items[index2]] = index1;
        }

        (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
    }

    public void Clear()
    {
        _items.Clear();
        _indexes.Clear();
        Count = 0;
    }
}
