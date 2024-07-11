using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentInventoryPopup : UI_Popup
{
    enum Texts
    {
        HPAmountText,
        MPAmountText,
        SPAmountText,
        DamageAmountText,
        DefenseAmountText,
    }

    enum Buttons
    {
        CloseButton,
    }

    private readonly Dictionary<EquipmentType, UI_EquipmentSlot> _equipmentSlots = new();

    protected override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        //Player.Status.HPChanged += RefreshHPText;
        //Player.Status.MPChanged += RefreshMPText;
        //Player.Status.SPChanged += RefreshSPText;
        //Player.Status.StatChanged += RefreshAllStatusText;

        InitSlots();
    }

    private void Start()
    {
        Managers.UI.Register<UI_EquipmentInventoryPopup>(this);

        GetButton((int)Buttons.CloseButton).onClick.AddListener(Managers.UI.Close<UI_EquipmentInventoryPopup>);
        Player.EquipmentInventory.InventoryChanged += equipmentType => _equipmentSlots[equipmentType].Refresh();
        RefreshAllStatusText();
    }

    private void RefreshAllStatusText()
    {
        //RefreshHPText();
        //RefreshMPText();
        //RefreshSPText();
        //RefreshDamageText();
        //RefreshDefenseText();
    }

    //private void RefreshHPText() => GetText((int)Texts.HPAmountText).text = $"체력 : {Player.Status.HP} / {Player.Status.MaxHP}";
    //private void RefreshMPText() => GetText((int)Texts.MPAmountText).text = $"마력 : {Player.Status.MP} / {Player.Status.MaxMP}";
    //private void RefreshSPText() => GetText((int)Texts.SPAmountText).text = $"기력 : {(int)Player.Status.SP} / {Player.Status.MaxSP}";
    //private void RefreshDamageText() => GetText((int)Texts.DamageAmountText).text = $"공격력 : {Player.Status.Damage}";
    //private void RefreshDefenseText() => GetText((int)Texts.DefenseAmountText).text = $"방어력 : {Player.Status.Defense}";

    private void InitSlots()
    {
        var equipmentSlots = GetComponentsInChildren<UI_EquipmentSlot>();
        foreach (var equipmentSlot in equipmentSlots)
        {
            _equipmentSlots.Add(equipmentSlot.EquipmentType, equipmentSlot);
        }
    }
}
