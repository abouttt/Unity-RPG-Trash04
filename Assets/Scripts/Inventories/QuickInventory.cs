using System;
using UnityEngine;

public class QuickInventory : MonoBehaviour
{
    public event Action<int> InventoryChanged;

    [field: SerializeField]
    public int Capacity { get; private set; }

    private readonly Inventory<IQuickable> _inventory = new();

    private void Awake()
    {
        _inventory.Init(Capacity);
    }

    public void SetQuickable(IQuickable quickable, int index)
    {
        if (_inventory.SetItem(quickable, index, 1))
        {
            if (quickable is Item item)
            {
                item.Destroyed -= OnItemDestroyed;
                item.Destroyed += OnItemDestroyed;
            }

            InventoryChanged?.Invoke(index);
        }
    }

    public void RemoveQuickable(int index)
    {
        var quickable = _inventory[index];

        if (_inventory.RemoveItem(index))
        {
            if (quickable is Item item)
            {
                if (!_inventory.IsIncluded(quickable))
                {
                    item.Destroyed -= OnItemDestroyed;
                }
            }

            InventoryChanged?.Invoke(index);
        }
    }

    public IQuickable GetQuickable(int index)
    {
        return _inventory.GetItem<IQuickable>(index);
    }

    public void SwapQuickable(int indexA, int indexB)
    {
        _inventory.SwapItem(indexA, indexB);
        InventoryChanged?.Invoke(indexA);
        InventoryChanged?.Invoke(indexB);
    }

    private void OnItemDestroyed(Item item)
    {
        var indexes = _inventory.GetItemAllIndex(item as IQuickable);
        foreach (var index in indexes)
        {
            RemoveQuickable(index);
        }
    }
}
