using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input/UIInput")]
public class UIInputReader : ScriptableObject, Controls.IUIActions
{
    public event Action EscEvent;
    public event Action TapEvent;
    public event Action XKeyEvent;

    private Controls _controls;
    public Controls Controls => _controls;

    public void ClearSubscription()
    {
        EscEvent = null;
        TapEvent = null;
        XKeyEvent = null;
    }

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

    public void OnXKey(InputAction.CallbackContext context)
    {
        if (context.performed)
            XKeyEvent?.Invoke();
    }
}

