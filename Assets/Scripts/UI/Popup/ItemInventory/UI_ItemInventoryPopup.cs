using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemInventoryPopup : UI_Popup
{
    enum RTs
    {
        EquipmentSlots,
        ConsumableSlots,
        MiscellaneousSlots
    }

    enum Texts
    {
        GoldText
    }

    enum Buttons
    {
        CloseButton,
    }

    enum ScrollRects
    {
        ItemSlotScrollView,
    }

    enum Tabs
    {
        EquipmentTabButton,
        ConsumableTabButton,
        MiscellaneousTabButton,
    }

    [SerializeField]
    private float _tabShowedXPosition;

    [SerializeField]
    private float _tabClosedXPosition;

    private readonly Dictionary<ItemType, UI_ItemSlot[]> _slots = new();
    private readonly Dictionary<UI_ItemInventoryTab, RectTransform> _tabs = new();

    protected override void Init()
    {
        base.Init();

        BindRT(typeof(RTs));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        Bind<ScrollRect>(typeof(ScrollRects));
        Bind<UI_ItemInventoryTab>(typeof(Tabs));

        InitSlots();
        InitTabs();
    }

    private void Start()
    {
        Managers.UI.Register<UI_ItemInventoryPopup>(this);

        Showed += () =>
        {
            PopupRT.SetParent(transform);
            Get<ScrollRect>((int)ScrollRects.ItemSlotScrollView).verticalScrollbar.value = 1f;
        };

        GetButton((int)Buttons.CloseButton).onClick.AddListener(Managers.UI.Close<UI_ItemInventoryPopup>);
        Player.ItemInventory.InventoryChanged += (itemType, index) => _slots[itemType][index].Refresh();
        ShowItemSlots(ItemType.Equipment);
    }

    public void ShowItemSlots(ItemType itemType)
    {
        var scrollView = Get<ScrollRect>((int)ScrollRects.ItemSlotScrollView);
        scrollView.verticalScrollbar.value = 1f;

        foreach (var kvp in _tabs)
        {
            var pos = kvp.Key.RectTransform.anchoredPosition;

            if (kvp.Key.TabType == itemType)
            {
                scrollView.content = kvp.Value;
                pos.x = _tabShowedXPosition;
                kvp.Value.gameObject.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(kvp.Value);
            }
            else
            {
                pos.x = _tabClosedXPosition;
                kvp.Value.gameObject.SetActive(false);
            }

            kvp.Key.RectTransform.anchoredPosition = pos;
        }
    }

    public void SetActiveCloseButton(bool active)
    {
        GetButton((int)Buttons.CloseButton).gameObject.SetActive(active);
    }

    private void InitSlots()
    {
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
        {
            var parent = GetRT((int)Enum.Parse(typeof(RTs), $"{itemType}Slots"));
            CreateItemSlots(itemType, Player.ItemInventory.Inventories[itemType].Capacity, parent);
            _slots.Add(itemType, parent.GetComponentsInChildren<UI_ItemSlot>());
        }
    }

    private void InitTabs()
    {
        foreach (Tabs tabType in Enum.GetValues(typeof(Tabs)))
        {
            var tab = Get<UI_ItemInventoryTab>((int)tabType);
            var rt = GetRT((int)Enum.Parse(typeof(RTs), $"{tab.TabType}Slots"));
            tab.GetComponent<Button>().onClick.AddListener(() =>
            {
                SetTop();
                ShowItemSlots(tab.TabType);
            });
            _tabs.Add(tab, rt);
        }
    }

    private void CreateItemSlots(ItemType itemType, int capacity, Transform parent)
    {
        for (int index = 0; index < capacity; index++)
        {
            var itemSlot = Managers.Resource.Instantiate<UI_ItemSlot>("UI_ItemSlot.prefab", parent);
            itemSlot.Setup(itemType, index);
        }
    }
}
