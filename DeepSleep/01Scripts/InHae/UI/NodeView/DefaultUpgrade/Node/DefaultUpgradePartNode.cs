using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class DefaultUpgradePartNode : BaseNode, IPointerClickHandler
{
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannel;
    
    public NodeEquipData CurrentEquipData { get; private set; }
    public NodeData CurrentNodeData { get; private set; }
    public bool isEmpty => CurrentEquipData?.partInventoryItem?.data == null;

    private bool _isConnect;
    
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
        var evt = DefaultNodeUpgradeEvents.UpgradeNodeSelectEvent;
        evt.selectNode = this;
        evt.isSelected = true;
        
        _defaultNodeEventChannel.RaiseEvent(evt);
    }
}
