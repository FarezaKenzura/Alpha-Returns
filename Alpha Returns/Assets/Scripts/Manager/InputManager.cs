using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum InputMap
{
    Player,
    UI
}

public enum InputType
{
    Submit,
    Pause,
    Cancel
}

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private Dictionary<InputType, bool> _inputStates = new();

    private void Awake()
    {
        SingletonHub.Instance.Register(this);
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Enum.TryParse(context.action.name, out InputType actionType))
        {
            _inputStates[actionType] = true;
        }
    }

    public bool GetActionPressed(InputType actionType)
    {
        if (_inputStates.TryGetValue(actionType, out bool wasPressed) && wasPressed)
        {
            _inputStates[actionType] = false;
            return true;
        }
        return false;
    }

    public void SwitchInputMap(InputMap newMap)
    {
        _playerInput.SwitchCurrentActionMap(newMap.ToString());
        _inputStates.Clear();
        Debug.Log($"[InputManager] Switched to {newMap}");
    }

    public bool IsGameplayActive() => _playerInput.currentActionMap.name == "Player";

    public bool IsPointerOverUI() => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
}
