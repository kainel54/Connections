using DG.Tweening;
using IH.EventSystem.UIEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;

public class SelectUpgradePanelUI : WindowPanel
{
    private void Awake()
    {
        _uiEventChannel.AddListener<UpgradePanelEvent>(HandleUpgradePanelEvent);
    }

    private void OnDestroy()
    {
        _uiEventChannel.RemoveListener<UpgradePanelEvent>(HandleUpgradePanelEvent);
    }

    private void HandleUpgradePanelEvent(UpgradePanelEvent evt)
    {
        if (evt.isPanelActive)
            HandleOpen();
        else
            HandleCloseUI();
    }
    
    public override void OpenWindow()
    {
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
    }

    public override void CloseWindow()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetUpdate(true);
    }

    private void HandleOpen()
    {
        var uiOpenEvent = UIPanelEvent.WindowPanelOpenEvent;
        uiOpenEvent.currentWindow = this;
        _uiEventChannel.RaiseEvent(uiOpenEvent);
    }
}
