using UnityEngine;

public class HPPotion : ConsumableItem
{
    public HPPotion(HPPotionData data, int count)
        : base(data, count)
    { }

    public override bool Use()
    {
        if (!CanUse())
        {
            return false;
        }

        Debug.Log("Use HP Potion");

        return true;
    }
}
