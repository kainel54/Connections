using System.Collections.Generic;
using IH.EventSystem.UIEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private UIInputReader _uiInput;
    [SerializeField] private PlayerInputSO _playerInputSO;
    
    [SerializeField] private PauseWindow pauseWindow;

    private List<WindowPanel> _openUIList = new ();
    
    private bool _isSetting;
    private bool _isOpenLocked;
    private bool _isCloseLocked;

    private void Awake()
    {
        _uiInput.EscEvent += HandleEscCheck;
        
        _uiEventChannelSO.AddListener<WindowPanelToggleEvent>(HandleWindowPanelToggleEvent);
        _uiEventChannelSO.AddListener<WindowPanelOpenEvent>(HandleOpenWindowPanel);
        _uiEventChannelSO.AddListener<WindowPanelCloseEvent>(HandleCloseWindowPanel);
        
        _uiEventChannelSO.AddListener<WindowPanelLockEvent>(HandleLockEvent);
    }
    
    private void OnDestroy()
    {
        _uiInput.EscEvent -= HandleEscCheck;
        
        _uiEventChannelSO.RemoveListener<WindowPanelToggleEvent>(HandleWindowPanelToggleEvent);
        _uiEventChannelSO.RemoveListener<WindowPanelOpenEvent>(HandleOpenWindowPanel);
        _uiEventChannelSO.RemoveListener<WindowPanelCloseEvent>(HandleCloseWindowPanel);
        
        _uiEventChannelSO.RemoveListener<WindowPanelLockEvent>(HandleLockEvent);
    }

    private void HandleOpenWindowPanel(WindowPanelOpenEvent evt)
    {
        if (_isOpenLocked || _openUIList.Contains(evt.currentWindow))
            return;
    
        _openUIList.Add(evt.currentWindow);
        evt.currentWindow.OpenWindow();
        InputAndTimeSet(false);
    }

    private void HandleCloseWindowPanel(WindowPanelCloseEvent evt)
    {
        if (_isCloseLocked || !_openUIList.Contains(evt.currentWindow))
            return;
        
        _openUIList.Remove(evt.currentWindow);
        evt.currentWindow.CloseWindow();

        if (_openUIList.Count != 0) 
            return;

        InputAndTimeSet(true);
    }

    private void HandleLockEvent(WindowPanelLockEvent evt)
    {
        _isOpenLocked = evt.isOpenLocked;
        _isCloseLocked = evt.isCloseLocked;
    }

    private void HandleWindowPanelToggleEvent(WindowPanelToggleEvent evt)
    {
        WindowPanel currentWindow = evt.currentWindow;
        ToggleWindow(currentWindow);
    }

    private void HandleEscCheck()
    {
        if (_isCloseLocked)
            return;

        // UI 가 열린 상태에서 ESC ( 맨 뒤 부터 닫기 )
        if (_openUIList.Count > 0)
        {
            WindowPanel window = _openUIList[^1];
            ToggleWindow(window);
            return;
        }
        
        if(pauseWindow.isTween || _isOpenLocked)
            return;
        
        if (_isSetting)
            pauseWindow.CloseWindow();
        else
            pauseWindow.OpenWindow();
        
        Time.timeScale = _isSetting ? 1f : 0f;
        InputAndTimeSet(_isSetting);
        _isSetting = !_isSetting;
    }

    private void ToggleWindow(WindowPanel window)
    {
        if (_isOpenLocked || _isCloseLocked)
            return;

        if (_openUIList.Contains(window))
        {
            window.CloseWindow();
            _openUIList.Remove(window);

            if (_openUIList.Count != 0) 
                return;

            InputAndTimeSet(true);
        }
        else
        {
            InputAndTimeSet(false);
            window.OpenWindow();
            _openUIList.Add(window);
        }
    }

    private void InputAndTimeSet(bool isActive)
    {        
        if(isActive)
            _playerInputSO.Controls.Enable();
        else
            _playerInputSO.Controls.Disable();
    }
}
