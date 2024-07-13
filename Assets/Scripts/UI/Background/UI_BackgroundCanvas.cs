using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BackgroundCanvas : UI_Base, IPointerDownHandler, IDropHandler
{
    [SerializeField, Space(10), TextArea]
    private string DestroyItemText;

    protected override void Init()
    {
        Managers.UI.Register<UI_BackgroundCanvas>(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Managers.Input.CursorLocked = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<UI_BaseSlot>(out var slot))
        {
            switch (slot.SlotType)
            {
                case SlotType.Item:
                    OnDropItemSlot(slot as UI_ItemSlot);
                    break;
                case SlotType.Equipment:
                    OnDropEquipmentSlot(slot as UI_EquipmentSlot);
                    break;
                case SlotType.Quick:
                    OnDropQuickSlot(slot as UI_QuickSlot);
                    break;
            }
        }
    }

    private void OnDropItemSlot(UI_ItemSlot itemSlot)
    {
        var item = itemSlot.ObjectRef as Item;
        string text = $"[{item.Data.ItemName}] {DestroyItemText}";
        Managers.UI.Show<UI_ConfirmationPopup>().SetEvent(() =>
        {
            Player.ItemInventory.RemoveItem(itemSlot.ItemType, itemSlot.Index);
        },
        text);
    }

    private void OnDropEquipmentSlot(UI_EquipmentSlot equipmentSlot)
    {
        var equipmentItem = equipmentSlot.ObjectRef as EquipmentItem;
        Player.EquipmentInventory.UnequipItem(equipmentSlot.EquipmentType);
        Player.ItemInventory.AddItem(equipmentItem.Data);
    }

    private void OnDropQuickSlot(UI_QuickSlot quickSlot)
    {
        Player.QuickInventory.RemoveQuickable(quickSlot.Index);
    }
}
