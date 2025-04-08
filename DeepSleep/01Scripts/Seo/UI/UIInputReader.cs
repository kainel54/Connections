using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input/UIInput")]
public class UIInputReader : ScriptableObject, Controls.IUIActions
{
    public Action EscEvent;
    public Action TapEvent;

    private Controls _controls;
    public Controls Controls => _controls;

    private bool _onSetting;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.UI.SetCallbacks(this);
        }

        _controls.UI.Enable();
    }

    private void OnDisable()
    {
        _controls.UI.Disable();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EscEvent?.Invoke();
        }
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TapEvent?.Invoke();
        }
    }
}

