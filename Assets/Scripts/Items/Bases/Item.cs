using UnityEngine;

public class Item : IItem
{
    public ItemData Data { get; private set; }

    public Item(ItemData itemData)
    {
        Data = itemData;
    }
}
