using UnityEngine;

public class UI_QuickInventoryFixed : UI_Base
{
    enum RTs
    {
        QuickSlots,
    }

    private UI_QuickSlot[] _quickSlots;

    protected override void Init()
    {
        Managers.UI.Register<UI_QuickInventoryFixed>(this);

        BindRT(typeof(RTs));

        InitSlots();
    }

    private void Start()
    {
        Player.QuickInventory.InventoryChanged += index => _quickSlots[index].Refresh();
    }

    private void InitSlots()
    {
        for (int i = 0; i < Player.QuickInventory.Capacity; i++)
        {
            var quickSlot = Managers.Resource.Instantiate<UI_QuickSlot>("UI_QuickSlot.prefab", GetRT((int)RTs.QuickSlots));
            quickSlot.Setup(i);
        }

        _quickSlots = GetRT((int)RTs.QuickSlots).GetComponentsInChildren<UI_QuickSlot>();
    }
}
