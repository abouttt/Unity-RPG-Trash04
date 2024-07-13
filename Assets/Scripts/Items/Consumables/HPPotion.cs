using UnityEngine;

public class HPPotion : ConsumableItem
{
    public HPPotionData HPPotionData { get; private set; }

    public HPPotion(HPPotionData data, int count)
        : base(data, count)
    {
        HPPotionData = data;
    }

    public override bool Use()
    {
        if (!base.Use())
        {
            return false;
        }

        Debug.Log($"Use HP Potion : {HPPotionData.HealAmount}");

        return true;
    }
}
