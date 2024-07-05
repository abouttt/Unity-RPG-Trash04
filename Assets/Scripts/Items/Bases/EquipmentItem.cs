using UnityEngine;

public class EquipmentItem : Item, IUsableItem
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

        Debug.Log($"Use {EquipmentData.ItemName}");

        return true;
    }

    public bool CanUse()
    {
        return true;
    }
}
