using DG.Tweening;
using UnityEngine;

public class SettingWindow : WindowPanel
{
    public void OpenHandle()
    {
        var evt = UIEvents.WindowPanelOpenEvent;
        evt.currentWindow = this;

        _uiEventChannel.RaiseEvent(evt);
    }
    
    public override void OpenWindow()
    {
        transform.DOScale(Vector3.one, 0.5f);
    }

    public override void CloseWindow()
    {
        transform.DOScale(Vector3.zero, 0.5f);
    }
}
