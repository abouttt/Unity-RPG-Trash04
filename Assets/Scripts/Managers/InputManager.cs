using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    public GameControls.PlayerActions Player => _gameControls.Player;
    public GameControls.UIActions UI => _gameControls.UI;

    public bool CursorLocked
    {
        get => _cursorLocked;
        set
        {
            _cursorLocked = value;
            Cursor.visible = !value;
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    private GameControls _gameControls;
    private bool _cursorLocked;

    public void Init()
    {
        _gameControls = new GameControls();
        _gameControls.Enable();
    }

    public InputActionMap GetActionMap(string nameOrId)
    {
        if (_gameControls == null)
        {
            Debug.Log("[InputManager/GetAction] Input system is null. Make sure it's activated.");
            return null;
        }

        return _gameControls.asset.FindActionMap(nameOrId);
    }

    public InputAction GetAction(string actionNameOrId)
    {
        if (_gameControls == null)
        {
            Debug.Log("[InputManager/GetAction] Input system is null. Make sure it's activated.");
            return null;
        }

        return _gameControls.FindAction(actionNameOrId);
    }

    public string GetBindingPath(string actionNameOrId, int bindingIndex = 0)
    {
        var key = GetAction(actionNameOrId).bindings[bindingIndex].path;
        var path = key.GetStringAfterLastSlash();
        return path.ToUpper();
    }
}
