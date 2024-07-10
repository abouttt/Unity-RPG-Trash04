using UnityEngine;
using UnityEngine.UI;

public class UI_ItemTooltip : UI_BaseTooltip
{
    enum Texts
    {
        ItemNameText,
        ItemTypeText,
        ItemDescText,
    }

    [Space(10)]
    [SerializeField]
    private Color _commonColor = Color.white;

    [SerializeField]
    private Color _uncommonColor = Color.white;

    [SerializeField]
    private Color _rareColor = Color.white;

    [SerializeField]
    private Color _epicColor = Color.white;

    [SerializeField]
    private Color _legendaryColor = Color.white;

    private ItemData _itemDataRef;

    protected override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
    }

    private void OnDisable()
    {
        _itemDataRef = null;
    }

    protected override void SetData()
    {
        if (SlotRef.ObjectRef is Item item)
        {
            SetItemData(item.Data);
        }
        else if (SlotRef.ObjectRef is ItemData itemData)
        {
            SetItemData(itemData);
        }
    }

    private void SetItemData(ItemData itemData)
    {
        if (_itemDataRef != null && _itemDataRef.Equals(itemData))
        {
            return;
        }

        _itemDataRef = itemData;
        GetText((int)Texts.ItemNameText).text = itemData.ItemName;
        SetItemRarityColor(itemData.ItemRarity);
        SetItemType(itemData.ItemType);
        SetDescription(itemData);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetRT((int)RTs.Tooltip));
    }

    private void SetItemRarityColor(ItemRarity itemRarity)
    {
        GetText((int)Texts.ItemNameText).color = itemRarity switch
        {
            ItemRarity.Common => _commonColor,
            ItemRarity.Uncommon => _uncommonColor,
            ItemRarity.Rare => _rareColor,
            ItemRarity.Epic => _epicColor,
            ItemRarity.Legendary => _legendaryColor,
            _ => Color.red,
        };
    }

    private void SetItemType(ItemType itemType)
    {
        GetText((int)Texts.ItemTypeText).text = itemType switch
        {
            ItemType.Equipment => "[장비 아이템]",
            ItemType.Consumable => "[소비 아이템]",
            ItemType.Miscellaneous => "[기타 아이템]",
            _ => "[NULL]"
        };
    }

    private void SetDescription(ItemData itemData)
    {
        SB.Clear();

        if (itemData is ILimitLevelable limitLevelableItemData)
        {
            SB.Append($"제한 레벨 : {limitLevelableItemData.LimitLevel}\n");
        }

        if (itemData is EquipmentItemData equipmentItemData)
        {
            //AppendValueIfGreaterThan0("체력", equipmentItemData.FixedStats.HP);
            //AppendValueIfGreaterThan0("마나", equipmentItemData.FixedStats.MP);
            //AppendValueIfGreaterThan0("기력", (int)equipmentItemData.FixedStats.SP);
            //AppendValueIfGreaterThan0("공격력", equipmentItemData.FixedStats.Damage);
            //AppendValueIfGreaterThan0("방어력", equipmentItemData.FixedStats.Defense);
        }
        else if (itemData is ConsumableItemData consumableItemData)
        {
            SB.Append($"소비 개수 : {consumableItemData.RequiredCount}\n");
        }

        if (SB.Length > 0)
        {
            SB.Append("\n");
        }

        if (!string.IsNullOrEmpty(itemData.Description))
        {
            SB.Append($"{itemData.Description}\n\n");
        }

        GetText((int)Texts.ItemDescText).text = SB.ToString();
    }

    private void AppendValueIfGreaterThan0(string text, int value)
    {
        if (value > 0)
        {
            SB.Append($"{text} +{value}\n");
        }
    }
}
