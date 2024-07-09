using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class FieldItem : Interactable
{
    public IReadOnlyDictionary<ItemData, int> Items => _items;

    [SerializeField]
    private bool _destroyWhenEmpty = true;

    [SerializeField]
    private SerializedDictionary<ItemData, int> _items;

    private void Start()
    {
        if ((_items == null || _items.Count == 0) && _destroyWhenEmpty)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }

    public override void Interact()
    {
        base.Interact();

        Managers.UI.Show<UI_LootPopup>().SetFieldItem(this);
    }

    public void AddItem(ItemData itemData, int count)
    {
        if (count <= 0)
        {
            return;
        }

        if (!_items.ContainsKey(itemData))
        {
            _items.Add(itemData, 0);
        }

        _items[itemData] += count;
    }

    public virtual void RemoveItem(ItemData itemData, int count)
    {
        if (count <= 0)
        {
            return;
        }

        if (_items.ContainsKey(itemData))
        {
            _items[itemData] -= count;

            if (_items[itemData] <= 0)
            {
                _items.Remove(itemData);
            }
        }

        if (_items.Count == 0 && _destroyWhenEmpty)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
}
