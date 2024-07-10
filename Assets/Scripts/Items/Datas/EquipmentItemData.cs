using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equipment", fileName = "Item_Equipment_")]
public class EquipmentItemData : ItemData, ILimitLevelableItemData
{
    [field: Header("Equipment Data")]

    [field: SerializeField]
    public int LimitLevel { get; private set; } = 1;

    [field: SerializeField]
    public EquipmentType EquipmentType { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    public EquipmentItemData()
        : base(ItemType.Equipment)
    { }

    public override Item CreateItem()
    {
        return new EquipmentItem(this);
    }
}
