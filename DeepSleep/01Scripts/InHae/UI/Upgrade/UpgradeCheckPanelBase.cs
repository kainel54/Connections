using System;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using UnityEngine.UI;
using YH.Players;

public abstract class UpgradeCheckPanelBase : WindowPanel
{
    [SerializeField] protected PlayerManagerSO _playerManagerSO;
    [SerializeField] protected Image _skillImage;
    
    protected GameObject _submitButton;
    protected GameObject _cancelButton;
    protected GameObject _checkButton;
    
    protected SkillInventoryItem _selectedItem;
    protected CanvasGroup _canvasGroup;
    
    public Action UpgradeEvent;

    protected virtual void Awake()
    {
        _submitButton = transform.Find("Buttons/SubmitButton").gameObject;
        _cancelButton = transform.Find("Buttons/CancelButton").gameObject;
        _checkButton = transform.Find("Buttons/CheckButton").gameObject;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        
        CloseWindow();
    }

    public override void OpenWindow()
    {
        _submitButton.SetActive(true);
        _cancelButton.SetActive(true);
        _checkButton.SetActive(false);
        
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public override void CloseWindow()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
    
    public override void HandleCloseUI()
    {
        var uiLockEvent = UIPanelEvent.WindowPanelLockEvent;
        uiLockEvent.isOpenLocked = false;
        _uiEventChannel.RaiseEvent(uiLockEvent);
        
        var evt = UIPanelEvent.WindowPanelToggleEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);
    }

    public virtual void Upgrade()
    {
        var uiLockEvent = UIPanelEvent.WindowPanelLockEvent;
        uiLockEvent.isOpenLocked = true;
        _uiEventChannel.RaiseEvent(uiLockEvent);
    }
}
