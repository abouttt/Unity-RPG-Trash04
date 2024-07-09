using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_LootPopup : UI_Popup
{
    enum RTs
    {
        LootSubitems,
    }

    enum Texts
    {
        LootAllText,
    }

    enum Buttons
    {
        CloseButton,
        LootAllButton,
    }

    [SerializeField]
    private float _trackingDistance;

    private FieldItem _fieldItemRef;
    private InputAction _interact;
    private readonly Dictionary<UI_LootSubitem, ItemData> _lootSubitems = new();

    protected override void Init()
    {
        base.Init();

        BindRT(typeof(RTs));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _interact = Managers.Input.GetAction("Interact");
    }

    private void Start()
    {
        Managers.UI.Register<UI_LootPopup>(this);

        Showed += () => _interact.performed += AddAllItemToItemInventoryInputAction;

        Closed += () =>
        {
            Clear();
            _interact.performed -= AddAllItemToItemInventoryInputAction;
        };

        GetText((int)Texts.LootAllText).text = $"[{Managers.Input.GetBindingPath("Interact")}] ¸ðµÎ È¹µæ";
        GetButton((int)Buttons.CloseButton).onClick.AddListener(Managers.UI.Close<UI_LootPopup>);
        GetButton((int)Buttons.LootAllButton).onClick.AddListener(AddAllItemToItemInventory);
    }

    private void Update()
    {
        if (_fieldItemRef == null)
        {
            Managers.UI.Close<UI_LootPopup>();
            return;
        }

        TrackingFieldItem();
    }

    public void SetFieldItem(FieldItem fieldItem)
    {
        _fieldItemRef = fieldItem;

        foreach (var kvp in _fieldItemRef.Items)
        {
            if (kvp.Key is StackableItemData stackableItemData)
            {
                int count = kvp.Value;
                while (count > 0)
                {
                    CreateLootSubitem(kvp.Key, Mathf.Clamp(count, count, stackableItemData.MaxCount));
                    count -= stackableItemData.MaxCount;
                }
            }
            else
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    CreateLootSubitem(kvp.Key, 1);
                }
            }
        }
    }

    public void AddItemToItemInventory(UI_LootSubitem lootSubitem)
    {
        _fieldItemRef.RemoveItem(lootSubitem.ItemDataRef, lootSubitem.Count);
        int count = Player.ItemInventory.AddItem(lootSubitem.ItemDataRef, lootSubitem.Count);
        if (count > 0)
        {
            lootSubitem.SetItemData(lootSubitem.ItemDataRef, count);
        }
        else
        {
            RemoveLootSubitem(lootSubitem);
        }
    }

    private void CreateLootSubitem(ItemData itemData, int count)
    {
        var lootSubitem = Managers.Resource.Instantiate<UI_LootSubitem>("UI_LootSubitem.prefab", GetRT((int)RTs.LootSubitems), true);
        lootSubitem.SetItemData(itemData, count);
        _lootSubitems.Add(lootSubitem, itemData);
    }

    private void RemoveLootSubitem(UI_LootSubitem lootSubitem)
    {
        _lootSubitems.Remove(lootSubitem);
        Managers.Resource.Destroy(lootSubitem.gameObject);
        if (_lootSubitems.Count == 0)
        {
            Managers.UI.Close<UI_LootPopup>();
        }
    }

    private void TrackingFieldItem()
    {
        if (_trackingDistance < Vector3.Distance(Player.GameObject.transform.position, _fieldItemRef.transform.position))
        {
            Managers.UI.Close<UI_LootPopup>();
        }
    }

    private void AddAllItemToItemInventory()
    {
        for (int i = _lootSubitems.Count - 1; i >= 0; i--)
        {
            AddItemToItemInventory(_lootSubitems.ElementAt(i).Key);
        }
    }

    private void Clear()
    {
        if (Managers.Instance != null)
        {
            foreach (var kvp in _lootSubitems)
            {
                Managers.Resource.Destroy(kvp.Key.gameObject);
            }
        }

        _lootSubitems.Clear();

        if (_fieldItemRef != null)
        {
            _fieldItemRef.Deinteract();
            _fieldItemRef = null;
        }
    }

    private void AddAllItemToItemInventoryInputAction(InputAction.CallbackContext context)
    {
        AddAllItemToItemInventory();
    }
}
