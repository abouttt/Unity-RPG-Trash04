using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : GameControls.IPlayerActions
{
    public bool CursorLocked
    {
        get => _cursorLocked;
        set
        {
            _cursorLocked = value;
            Cursor.visible = !value;
            SetCursorState(value);
        }
    }

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;

            if (_enabled)
            {
                if (_controls != null)
                {
                    Enabled = false;
                }

                _controls = new();
                _controls.Player.SetCallbacks(this);
                _controls.Enable();
            }
            else
            {
                if (_controls == null)
                {
                    return;
                }

                _controls.Disable();
                _controls.Dispose();
                _controls = null;
            }
        }
    }

    private GameControls _controls;
    private bool _cursorLocked;
    private bool _enabled;

    public InputAction GetAction(string actionNameOrId)
    {
        if (_controls == null)
        {
            Debug.Log("[InputManager/GetAction] Input system is null. Make sure it's activated.");
            return null;
        }

        return _controls.FindAction(actionNameOrId);
    }

    public string GetBindingPath(string actionNameOrId, int bindingIndex = 0)
    {
        var key = GetAction(actionNameOrId).bindings[bindingIndex].path;
        return key.GetStringAfterLastSlash().ToUpper();
    }

    public void Clear()
    {
        Enabled = false;
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
