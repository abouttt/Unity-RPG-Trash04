using UnityEngine.EventSystems;

public class UI_ItemSlot : UI_BaseSlot, IDropHandler
{
    enum Texts
    {
        CountText,
    }

    enum CooldownImages
    {
        CooldownImage,
    }

    public ItemType ItemType { get; private set; }
    public int Index { get; private set; }

    protected override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        Bind<UI_CooldownImage>(typeof(CooldownImages));
    }

    private void Start()
    {
        Refresh();
    }

    public void Setup(ItemType itemType, int index)
    {
        ItemType = itemType;
        Index = index;
    }

    public void Refresh()
    {
        var item = Player.ItemInventory.GetItem<Item>(ItemType, Index);
        if (item != null)
        {
            if (ObjectRef != item)
            {
                if (HasObject)
                {
                    Clear();
                }

                SetObject(item, item.Data.ItemImage);

                if (item is IStackableItem stackableItem)
                {
                    stackableItem.StackChanged += RefreshCountText;
                }

                if (item.Data is ICooldownable cooldownable)
                {
                    Get<UI_CooldownImage>((int)CooldownImages.CooldownImage).SetCooldown(cooldownable.Cooldown);
                }

                RefreshCountText();
            }
        }
        else
        {
            Clear();
        }
    }

    protected override void Clear()
    {
        if (ObjectRef is Item item)
        {
            if (item is IStackableItem stackableItem)
            {
                stackableItem.StackChanged -= RefreshCountText;
            }

            if (item.Data is ICooldownable)
            {
                Get<UI_CooldownImage>((int)CooldownImages.CooldownImage).Clear();
            }
        }

        base.Clear();
        GetText((int)Texts.CountText).gameObject.SetActive(false);
    }

    private void RefreshCountText()
    {
        if (ObjectRef is IStackableItem stackableItem && stackableItem.Count > 1)
        {
            GetText((int)Texts.CountText).gameObject.SetActive(true);
            GetText((int)Texts.CountText).text = stackableItem.Count.ToString();
        }
        else
        {
            GetText((int)Texts.CountText).gameObject.SetActive(false);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Managers.UI.Get<UI_ItemInventoryPopup>().SetTop();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }

    public override void OnPointerExit(PointerEventData eventData)
    {

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!CanPointerUp())
        {
            return;
        }

        if (ObjectRef is IUsableItem usableItem)
        {
            usableItem.Use();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
        {
            return;
        }

        if (eventData.pointerDrag.TryGetComponent<UI_BaseSlot>(out var otherSlot))
        {
            switch (otherSlot.SlotType)
            {
                case SlotType.Item:
                    OnDropItemSlot(otherSlot as UI_ItemSlot);
                    break;
            }
        }
    }

    private void OnDropItemSlot(UI_ItemSlot otherItemSlot)
    {
        var otherItem = otherItemSlot.ObjectRef as Item;
        if (!HasObject && otherItem is IStackableItem otherStackableItem && otherStackableItem.Count > 1)
        {
            var splitPopup = Managers.UI.Show<UI_ItemSplitPopup>();
            splitPopup.SetEvent(() =>
                Player.ItemInventory.SplitItem(ItemType, otherItemSlot.Index, Index, splitPopup.Count),
                $"[{otherItem.Data.ItemName}] ������ ������", 1, otherStackableItem.Count);
        }
        else
        {
            Player.ItemInventory.MoveItem(ItemType, otherItemSlot.Index, Index);
        }

    }
}
