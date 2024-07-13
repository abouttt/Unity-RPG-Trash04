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
            InventoryChanged?.Invoke(index);
        }
    }

    public void RemoveQuickable(int index)
    {
        if (_inventory.RemoveItem(index))
        {
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
}
