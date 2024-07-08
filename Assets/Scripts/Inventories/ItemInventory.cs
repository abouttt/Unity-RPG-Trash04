using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class ItemInventory : MonoBehaviour
{
    public event Action<ItemType, int> InventoryChanged;

    public IReadOnlyDictionary<ItemType, Inventory> Inventories => _inventories;

    [SerializeField, SerializedDictionary("Item Type", "Inventory")]
    private SerializedDictionary<ItemType, Inventory> _inventories;

    private void Awake()
    {
        foreach (var kvp in _inventories)
        {
            kvp.Value.Init();
        }
    }

    public int AddItem(ItemData itemData, int count = 1)
    {
        if (itemData == null)
        {
            Debug.LogWarning("[ItemInventory/AddItem] ItemData is null.");
            return -1;
        }

        if (count <= 0)
        {
            Debug.LogWarning($"[ItemInventory/AddItem] {itemData.ItemName} count <= 0");
            return -1;
        }

        var inventory = _inventories[itemData.ItemType];
        var stackableItemData = itemData as StackableItemData;
        bool isStackable = stackableItemData != null;

        // 같은 아이템에 개수 더하기 시도
        if (isStackable)
        {
            for (int index = 0; index < inventory.Capacity; index++)
            {
                if (count <= 0)
                {
                    break;
                }

                int sameItemIndex = inventory.FindSameItemIndex(stackableItemData, index);
                if (sameItemIndex != -1)
                {
                    var sameItem = inventory.GetItem<StackableItem>(sameItemIndex);
                    count = sameItem.IsMax ? count : sameItem.AddCountAndGetExcess(count);
                    index = sameItemIndex;
                }
                else
                {
                    break;
                }
            }
        }

        // 빈 공간에 아이템 추가 시도
        for (int index = 0; index < inventory.Capacity; index++)
        {
            if (count <= 0)
            {
                break;
            }

            int emptyIndex = inventory.FindEmptyIndex(index);
            if (emptyIndex != -1)
            {
                SetItem(itemData, emptyIndex, count);
                count = isStackable ? Mathf.Max(0, count - stackableItemData.MaxCount) : count - 1;
                index = emptyIndex;
            }
            else
            {
                break;
            }
        }

        return count;
    }

    public void RemoveItem(Item item)
    {
        if (item == null)
        {
            return;
        }

        var itemType = item.Data.ItemType;
        int index = _inventories[itemType].GetItemIndex(item);
        if (index != -1)
        {
            _inventories[itemType].RemoveItem(index);
            InventoryChanged?.Invoke(itemType, index);
        }
    }

    public void RemoveItem(ItemType itemType, int index)
    {
        if (_inventories[itemType].RemoveItem(index))
        {
            InventoryChanged?.Invoke(itemType, index);
        }
    }

    public void SetItem(ItemData itemData, int index, int count = 1)
    {
        if (_inventories[itemData.ItemType].SetItem(itemData, index, count))
        {
            InventoryChanged?.Invoke(itemData.ItemType, index);
        }
    }
}
