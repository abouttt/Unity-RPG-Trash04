using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour, GameControls.IUIActions
{
    public void OnEnable()
    {
        Managers.Input.UI.SetCallbacks(this);
        Managers.Input.UI.Enable();
    }

    public void OnDisable()
    {
        if (Managers.Instance != null)
        {
            Managers.Input.UI.RemoveCallbacks(this);
            Managers.Input.UI.Disable();
        }
    }

    public void OnItemInventory(InputAction.CallbackContext context)
    {
        ShowOrClosePopup<UI_ItemInventoryPopup>(context);
    }

    public void OnEquipmentInventory(InputAction.CallbackContext context)
    {
        ShowOrClosePopup<UI_EquipmentInventoryPopup>(context);
    }

    public void OnQuick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Managers.UI.IsShowedHelperPopup || Managers.UI.IsShowedSelfishPopup)
            {
                return;
            }

            int index = (int)context.ReadValue<float>();
            var quickable = Player.QuickInventory.GetQuickable(index);
            quickable?.UseQuick();
        }
    }

    public void OnCursorToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Managers.UI.IsShowedSelfishPopup)
            {
                return;
            }

            Managers.Input.CursorLocked = !Managers.Input.CursorLocked;
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Managers.UI.ActivePopupCount > 0)
            {
                Managers.UI.CloseTopPopup();
            }
        }
    }

    private void ShowOrClosePopup<T>(InputAction.CallbackContext context) where T : UI_Popup
    {
        if (context.performed)
        {
            if (Managers.UI.IsShowedHelperPopup)
            {
                return;
            }

            Managers.UI.ShowOrClose<T>();
        }
    }
}
