using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : GameControls.IUIActions
{
    public void Enable()
    {
        Managers.Input.UI.SetCallbacks(this);
        Managers.Input.UI.Enable();
    }

    public void Disable()
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
