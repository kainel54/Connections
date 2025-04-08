using DG.Tweening;
using UnityEngine;

public class UpgradePanelUI : WindowPanel
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
        {
            // var openEvt = UIEvents.WindowPanelOpenEvent;
            // openEvt.currentWindow = this;
            // _uiEventChannel.RaiseEvent(openEvt);
            OpenWindow();
        }
        else
        {
            // var openEvt = UIEvents.WindowPanelCloseEvent;
            // _uiEventChannel.RaiseEvent(openEvt);
            CloseWindow();
        }
    }
    
    public override void OpenWindow()
    {
        transform.DOScale(Vector3.one, 0.3f);
    }

    public override void CloseWindow()
    {
        transform.DOScale(Vector3.zero, 0.3f);
    }
}
