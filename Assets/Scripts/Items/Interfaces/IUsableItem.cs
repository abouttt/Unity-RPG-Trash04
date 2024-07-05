using UnityEngine;

public interface IUsableItem : IItem
{
    public bool Use();
    public bool CanUse();
}
