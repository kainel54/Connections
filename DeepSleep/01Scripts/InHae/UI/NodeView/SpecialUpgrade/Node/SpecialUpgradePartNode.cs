using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YH.EventSystem;

public class SpecialUpgradePartNode : BaseNode, IPointerClickHandler
{
    [SerializeField] private Color _specialColor;
    
    [SerializeField] private Transform _edgeParent;
    [SerializeField] private GameEventChannelSO _specialNodeUpgradeEventChannel;
    public NodeEquipData CurrentEquipData { get; private set; }
    public NodeData CurrentNodeData { get; private set; }
    public bool isEmpty => CurrentEquipData?.partInventoryItem?.data == null;

    private bool _isConnect;

    private List<Image> _edgeImageList = new List<Image>();

    protected override void Awake()
    {
        base.Awake();
        _edgeImageList = _edgeParent.GetComponentsInChildren<Image>().ToList();
    }

    public void Init(NodeData nodeData, NodeEquipData nodeEquipData)
    {
        CurrentNodeData = nodeData;
        CurrentEquipData = nodeEquipData;
        
        gameObject.SetActive(true);
        UpdateNode(nodeEquipData);
    }

    private void UpdateNode(NodeEquipData nodeEquipData)
    {
        if (isEmpty)
            UpdateSlotImage(null, Color.clear);
        else
            UpdateSlotImage(nodeEquipData.partInventoryItem.data.icon, Color.white);
    }
    
    private void UpdateSlotImage(Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.color = color;
    }

    public override void NodeConnectCheck()
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            var node = connectedNodes[i] as DefaultUpgradePartNode;
            if (node.isEmpty)
                continue;
            
            _uiLineRenderers[i].LineEnable();
            if (_isConnect) 
                continue;

            node.activeFrame.ActiveFrameEnable();
            _isConnect = true;
            node.NodeConnectCheck();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(CurrentNodeData.isSpecial)
            return;
        
        var evt = SpecialNodeUpgradeEvents.UpgradeNodeSelectEvent;
        evt.selectNode = this;
        evt.isSelected = true;
        
        _specialNodeUpgradeEventChannel.RaiseEvent(evt);
    }

    public void ChangeColor(float time)
    {
        for (int i = 0; i < _edgeImageList.Count; i++)
        {
            Tween tween = _edgeImageList[i].DOColor(_specialColor, time);
        }
    }
}
