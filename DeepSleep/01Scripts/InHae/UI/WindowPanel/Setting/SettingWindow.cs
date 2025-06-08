using DG.Tweening;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;

public class SettingWindow : WindowPanel
{
    public void OpenHandle()
    {
        var evt = UIPanelEvent.WindowPanelToggleEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);
    }
    
    public override void OpenWindow()
    {
        transform.DOScale(Vector3.one, 0.5f).SetUpdate(true);
    }

    public override void CloseWindow()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true);
    }
}
