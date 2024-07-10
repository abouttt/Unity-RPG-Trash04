using System;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public event Action<EquipmentType> InventoryChanged;

    private readonly Inventory _inventory = new();

    private void Awake()
    {
        _inventory.Init(Enum.GetValues(typeof(EquipmentType)).Length);
    }

    public void Equip(EquipmentItemData equipmentItemData)
    {
        var equipmentType = equipmentItemData.EquipmentType;
        if (IsEquipped(equipmentType))
        {
            Unequip(equipmentType);
        }

        _inventory.SetItem(equipmentItemData, (int)equipmentType, 1);
        InventoryChanged?.Invoke(equipmentType);
    }

    public void Unequip(EquipmentType equipmentType)
    {
        if (!IsEquipped(equipmentType))
        {
            return;
        }

        _inventory.RemoveItem((int)equipmentType);
        InventoryChanged?.Invoke(equipmentType);
    }

    public EquipmentItem GetItem(EquipmentType equipmentType)
    {
        return _inventory.GetItem<EquipmentItem>((int)equipmentType);
    }

    public bool IsEquipped(EquipmentType equipmentType)
    {
        return _inventory.IsEmptyIndex((int)equipmentType);
    }
}
