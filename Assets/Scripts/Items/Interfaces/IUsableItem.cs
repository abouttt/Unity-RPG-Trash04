using UnityEngine;

public interface IUsableItem : IItem
{
    bool Use();
    bool CanUse();
}
