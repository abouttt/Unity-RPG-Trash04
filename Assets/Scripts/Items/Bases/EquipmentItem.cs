using UnityEngine;

public class EquipmentItem : Item, IUsable
{
    public EquipmentItemData EquipmentData { get; private set; }

    public EquipmentItem(EquipmentItemData data)
        : base(data)
    {
        EquipmentData = data;
    }

    public bool Use()
    {
        if (!CanUse())
        {
            return false;
        }

        var equippedItem = Player.EquipmentInventory.GetItem(EquipmentData.EquipmentType);
        if (equippedItem == this)
        {
            Player.EquipmentInventory.UnequipItem(EquipmentData.EquipmentType);
            Player.ItemInventory.AddItem(EquipmentData);
        }
        else
        {
            int index = Player.ItemInventory.GetItemIndex(this);
            if (equippedItem != null)
            {
                Player.ItemInventory.SetItem(equippedItem.EquipmentData, index);
            }
            else
            {
                Player.ItemInventory.RemoveItem(Data.ItemType, index);
            }
            Player.EquipmentInventory.EquipItem(EquipmentData);
        }

        return true;
    }

    public bool CanUse()
    {
        return true;
    }
}
