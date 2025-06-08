using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using YH.EventSystem;

public class NodeViewCloseButton : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private WindowPanel _nodeViewUI;
    
    public void HandleCloseNodeView()
    {
        var nodeViewOpenEvt = UIPanelEvent.WindowPanelCloseEvent;
        nodeViewOpenEvt.currentWindow = _nodeViewUI;
        _uiEventChannelSO.RaiseEvent(nodeViewOpenEvt);
    }
}
