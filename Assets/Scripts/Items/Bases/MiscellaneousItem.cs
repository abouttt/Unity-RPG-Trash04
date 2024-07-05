using UnityEngine;

public class MiscellaneousItem : StackableItem
{
    public MiscellaneousItemData MiscellaneousData { get; private set; }

    public MiscellaneousItem(MiscellaneousItemData data, int count)
        : base(data, count)
    {
        MiscellaneousData = data;
    }
}
