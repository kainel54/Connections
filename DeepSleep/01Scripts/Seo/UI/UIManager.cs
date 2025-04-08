using System.Collections.Generic;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private UIInputReader _uiInput;
    [SerializeField] private PlayerInputSO _playerInputSO;
    [SerializeField] private PauseWindow pauseWindow;

    public Stack<WindowPanel> _openUIStack = new Stack<WindowPanel>();
    private bool _isSetting;

    private void Awake()
    {
        _uiInput.EscEvent += HandleEscCheck;
        _uiEventChannelSO.AddListener<WindowPanelOpenEvent>(HandleWindowPanelEvent);
        _uiEventChannelSO.AddListener<WindowPanelCloseEvent>(HandleWindowPanelCloseEvent);
    }

    private void OnDestroy()
    {
        _uiInput.EscEvent -= HandleEscCheck;
        _uiEventChannelSO.RemoveListener<WindowPanelOpenEvent>(HandleWindowPanelEvent);
        _uiEventChannelSO.RemoveListener<WindowPanelCloseEvent>(HandleWindowPanelCloseEvent);
    }
    
    private void HandleWindowPanelCloseEvent(WindowPanelCloseEvent evt)
    {
        HandleEscCheck();
    }

    private void HandleWindowPanelEvent(WindowPanelOpenEvent evt)
    {
        if (_openUIStack.Contains(evt.currentWindow))
            return;

        if (evt.currentWindow as PauseWindow && _isSetting)
        {
            _isSetting = false;
            return;
        }

        _playerInputSO.Controls.Player.Disable();

        evt.currentWindow.OpenWindow();
        _openUIStack.Push(evt.currentWindow);
    }

    public void HandleEscCheck()
    {
        if (_openUIStack.Count <= 0)
        {
            _playerInputSO.Controls.Player.Disable();
            pauseWindow.OpenWindow();
            _openUIStack.Push(pauseWindow);
        }
        else if (_openUIStack.Count > 0)
        {
            WindowPanel ui = _openUIStack.Peek();

            if (ui as PauseWindow)
                _isSetting = true;

            ui.CloseWindow();
            _openUIStack.Pop();

            if (_openUIStack.Count == 0)
                _playerInputSO.Controls.Player.Enable();
        }
    }
}
