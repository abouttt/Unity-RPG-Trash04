using System;
using UnityEngine;

public static class ItemFactory
{
    public static EquipmentItem CreateEquipmentItem(EquipmentItemData data)
    {
        return new EquipmentItem(data);
    }

    public static MiscellaneousItem CreateMiscellaneousItem(MiscellaneousItemData data, int count)
    {
        return new MiscellaneousItem(data, count);
    }

    public static ConsumableItem CreateConsumableItem<T>(ConsumableItemData data, int count) where T : ConsumableItem
    {
        return Activator.CreateInstance(typeof(T), data, count) as ConsumableItem;
    }
}
