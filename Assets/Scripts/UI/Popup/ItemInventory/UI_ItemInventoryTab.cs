using UnityEngine;

public class UI_ItemInventoryTab : UI_Base
{
    [field: SerializeField]
    public ItemType TabType { get; private set; }

    public RectTransform RectTransform { get; private set; }

    protected override void Init()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}
