using UnityEngine;

[CreateAssetMenu(menuName = "Item/Miscellaneous", fileName = "Item_Miscellaneous_")]
public class MiscellaneousItemData : StackableItemData
{
    public MiscellaneousItemData()
        : base(ItemType.Miscellaneous)
    { }

    public override Item CreateItem()
    {
        return new MiscellaneousItem(this, 1);
    }

    public override Item CreateItem(int count)
    {
        return new MiscellaneousItem(this, count);
    }
}
