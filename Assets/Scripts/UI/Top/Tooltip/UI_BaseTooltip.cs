using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class UI_BaseTooltip : UI_Base
{
    protected enum GameObjects
    {
        Tooltip,
    }

    protected enum RTs
    {
        Tooltip,
    }

    protected UI_BaseSlot SlotRef;
    protected readonly StringBuilder SB = new(50);

    private bool _isShowed;

    protected override void Init()
    {
        BindObject(typeof(GameObjects));
        BindRT(typeof(RTs));
    }

    private void Update()
    {
        if (SlotRef == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (!SlotRef.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }

        if (UI_BaseSlot.IsDragging)
        {
            GetObject((int)GameObjects.Tooltip).SetActive(false);
            return;
        }

        if (SlotRef.HasObject)
        {
            if (!_isShowed)
            {
                GetObject((int)GameObjects.Tooltip).SetActive(true);
                SetData();
                _isShowed = true;
            }
        }
        else
        {
            if (_isShowed)
            {
                GetObject((int)GameObjects.Tooltip).SetActive(false);
                _isShowed = false;
            }
        }

        SetPosition(Mouse.current.position.ReadValue());
    }

    public void SetSlot(UI_BaseSlot slot)
    {
        if (SlotRef == slot)
        {
            return;
        }

        SlotRef = slot;
        gameObject.SetActive(slot != null);
        _isShowed = false;
        GetObject((int)GameObjects.Tooltip).SetActive(false);
    }

    protected abstract void SetData();

    private void SetPosition(Vector3 position)
    {
        var rt = GetRT((int)RTs.Tooltip);
        rt.position = new Vector3()
        {
            x = position.x + (rt.rect.width * 0.5f),
            y = position.y + (rt.rect.height * 0.5f)
        };
    }
}
