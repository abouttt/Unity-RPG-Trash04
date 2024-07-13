using UnityEngine;

public abstract class ConsumableItem : StackableItem, IUsable, IQuickable
{
    public ConsumableItemData ConsumableData { get; private set; }

    public ConsumableItem(ConsumableItemData data, int count)
        : base(data, count)
    {
        ConsumableData = data;
    }

    public virtual bool Use()
    {
        if (!CanUse())
        {
            return false;
        }

        Count -= ConsumableData.RequiredCount;
        if (IsEmpty)
        {
            Player.ItemInventory.RemoveItem(this);
        }

        Managers.Cooldown.AddCooldown(ConsumableData.Cooldown);

        return true;
    }

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

    public bool UseQuick()
    {
        return Use();
    }
}
