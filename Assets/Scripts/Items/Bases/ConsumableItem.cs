using UnityEngine;

public abstract class ConsumableItem : StackableItem, IUsable
{
    public ConsumableItemData ConsumableData { get; private set; }

    public ConsumableItem(ConsumableItemData data, int count)
        : base(data, count)
    {
        ConsumableData = data;
    }

    public abstract bool Use();

    public bool CanUse()
    {
        if (Count < ConsumableData.RequiredCount)
        {
            return false;
        }

        if (ConsumableData.Cooldown.RemainingTime > 0f)
        {
            return false;
        }

        return true;
    }

    protected void SubtractCountAndStartCooldown()
    {
        Count -= ConsumableData.RequiredCount;
        Managers.Cooldown.AddCooldown(ConsumableData.Cooldown);
    }
}
