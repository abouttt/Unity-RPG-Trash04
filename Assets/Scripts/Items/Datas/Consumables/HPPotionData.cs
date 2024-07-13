using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable/HP Potion", fileName = "Item_Consumable_HP_Potion")]
public class HPPotionData : ConsumableItemData
{
    [field: SerializeField]
    public int HealAmount { get; private set; }

    public override Item CreateItem()
    {
        return new HPPotion(this, 1);
    }

    public override Item CreateItem(int count)
    {
        return new HPPotion(this, count);
    }
}
