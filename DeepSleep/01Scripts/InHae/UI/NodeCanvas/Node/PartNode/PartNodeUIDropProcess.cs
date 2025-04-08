using IH.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartNodeUIDropProcess : MonoBehaviour, IDropHandler
{
    private PartNodeUI _partNodeUI;
    private PartNodeChainCheck _partNodeChainCheck;
    
    private void Awake()
    {
        _partNodeUI = GetComponent<PartNodeUI>();
        _partNodeChainCheck = GetComponent<PartNodeChainCheck>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;

        if (_partNodeUI.isChainMode)
        {
            DropIsChainMode(eventData);
            return;
        }
        
        // 인벤에서 노드로 오는 경우 
        DropIsInventory(eventData);
        // 노드에서 노드로 오는 경우 
        DropIsNode(eventData);
    }

    private void DropIsChainMode(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        PartItemSlotUI partSlotUI = gameObject.GetComponent<PartItemSlotUI>();

        if (partSlotUI == null || partSlotUI.isEmpty || !_partNodeChainCheck.isChainAble)
            return;

        _partNodeChainCheck.AddChainItem(partSlotUI.item as PartInventoryItem);
        _partNodeChainCheck.FindChainNode();

        InventoryManager.Instance.RemoveInventoryItemWithSo(partSlotUI.item.data);
    }

    private void DropIsNode(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        
        // 드래그를 시작한 노드
        PartNodeUI node = gameObject.GetComponent<PartNodeUI>();
        if(node == null)
            return;

        NodeData nodeData = _partNodeUI.CurrentData;
        
        _partNodeUI.UnEquipCurrentPart();
        _partNodeUI.UpdateNode(node.CurrentData);
        
        node.UnEquipCurrentPart();
        node.UpdateNode(nodeData);
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        dragItem.successDrop = true;

        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Chain);
        popUp.EndPopUp();
        
        _partNodeUI.skillNode.StartNodeCheck();
    }

    private void DropIsInventory(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        PartItemSlotUI partSlotUI = gameObject.GetComponent<PartItemSlotUI>();
        if (partSlotUI != null && !partSlotUI.isEmpty)
        {
            if (!_partNodeUI.isEmpty)
                InventoryManager.Instance.AddInventoryItemWithSo(_partNodeUI.CurrentData.partInventoryItem.data);
            
            _partNodeUI.UnEquipCurrentPart();

            NodeData nodeData = new NodeData
            {
                partInventoryItem = partSlotUI.item as PartInventoryItem
            };

            _partNodeUI.UpdateNode(nodeData);
            _partNodeUI.skillNode.StartNodeCheck();
            
            InventoryManager.Instance.RemoveInventoryItemWithSo(partSlotUI.item.data);
        }
    }
}
