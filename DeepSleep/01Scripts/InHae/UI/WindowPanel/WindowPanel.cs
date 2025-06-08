using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using YH.EventSystem;

public abstract class WindowPanel : MonoBehaviour
{
    [SerializeField] protected GameEventChannelSO _uiEventChannel;

    public abstract void OpenWindow();
    public abstract void CloseWindow();

    public virtual void HandleCloseUI()
    {
        var evt = UIPanelEvent.WindowPanelCloseEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);
    }
    
    public virtual void HandleOpenUI()
    {
        var evt = UIPanelEvent.WindowPanelOpenEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);
    }
}
