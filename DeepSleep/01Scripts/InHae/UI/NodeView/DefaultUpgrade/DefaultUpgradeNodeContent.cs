using System;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class DefaultUpgradeNodeContent : BaseNodeContent, IBeginDragHandler
{
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannel;
    
    private void Start()
    {
        _defaultNodeEventChannel.AddListener<UpgradeCountInitEvent>(HandleUpgradeContentInitEvent);
        
        var evt = DefaultNodeUpgradeEvents.NodeParentInitEvent;
        evt.parent = _visual;
        _defaultNodeEventChannel.RaiseEvent(evt);
    }

    private void OnDestroy()
    {
        _defaultNodeEventChannel.RemoveListener<UpgradeCountInitEvent>(HandleUpgradeContentInitEvent);
    }
    
    private void HandleUpgradeContentInitEvent(UpgradeCountInitEvent evt)
    {
        _visual.localPosition = Vector3.zero;
        _visual.localScale = Vector3.one;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var evt = DefaultNodeUpgradeEvents.UpgradeNodeSelectEvent;
        evt.selectNode = null;
        evt.isSelected = false;
        _defaultNodeEventChannel.RaiseEvent(evt);
    }
}
