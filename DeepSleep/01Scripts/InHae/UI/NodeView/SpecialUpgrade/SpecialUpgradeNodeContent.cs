using System;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class SpecialUpgradeNodeContent : BaseNodeContent, IBeginDragHandler
{
    [SerializeField] private GameEventChannelSO _specialUpgradeNodeEventChannel;
    
    private void Start()
    {
        _specialUpgradeNodeEventChannel.AddListener<UpgradeNodeInitEvent>(HandleSpecialUpgradeNodeInitEvent);
        
        var evt = SpecialNodeUpgradeEvents.NodeParentInitEvent;
        evt.parent = _visual;
        _specialUpgradeNodeEventChannel.RaiseEvent(evt);
    }

    private void OnDestroy()
    {
        _specialUpgradeNodeEventChannel.RemoveListener<UpgradeNodeInitEvent>(HandleSpecialUpgradeNodeInitEvent);
    }

    private void HandleSpecialUpgradeNodeInitEvent(UpgradeNodeInitEvent evt)
    {
        _visual.localPosition = Vector3.zero;
        _visual.localScale = Vector3.one;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var evt = SpecialNodeUpgradeEvents.UpgradeNodeSelectEvent;
        evt.selectNode = null;
        evt.isSelected = false;
        _specialUpgradeNodeEventChannel.RaiseEvent(evt);
    }
}
