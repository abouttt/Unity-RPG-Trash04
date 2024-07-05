using UnityEngine;

[CreateAssetMenu(menuName = "Item/Miscellaneous", fileName = "Item_Miscellaneous_")]
public class MiscellaneousItemData : StackableItemData
{
    public MiscellaneousItemData()
        : base(ItemType.Miscellaneous)
    {
    }
}
