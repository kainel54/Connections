using IH.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpecialPartNodeUIDrop : PartNodeUIDrop
{
    private SpecialPartNodeUI _specialPartNodeUI;
    private NodeAbilityInventoryItem _currentNodeInventory => _partNodeUI.CurrentEquipData.nodeAbilityInventoryItem;
    private bool _isEmpty => _currentNodeInventory == null || _currentNodeInventory.data == null;
    
    public override void Initialize(PartNodeUI partNodeUI)
    {
        base.Initialize(partNodeUI);
        _specialPartNodeUI = partNodeUI as SpecialPartNodeUI;
        _partNodeUIOnPopUp = _specialPartNodeUI.GetCompo<SpecialPartNodeUIOnPopUp>();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        DropIsInventoryNodeAbility(eventData);
    }

    private void DropIsInventoryNodeAbility(PointerEventData eventData)
    {
        GameObject dragTarget = eventData.pointerDrag;
        NodeAbilitySlotUI nodeAbilitySlotUI = dragTarget.GetComponent<NodeAbilitySlotUI>();
        if (nodeAbilitySlotUI != null && !nodeAbilitySlotUI.isEmpty)
        {
            if (!_specialPartNodeUI.isAbilityEmpty && !_isEmpty)
            {
                InventoryManager.Instance.AddInventoryItemWithSo(_currentNodeInventory.data);
                _specialPartNodeUI.UnEquipCurrentPart();
            }
            
            NodeEquipData nodeEquipData;
            if (_specialPartNodeUI.CurrentEquipData == null)
            {
                nodeEquipData = new NodeEquipData
                {
                    nodeAbilityInventoryItem = nodeAbilitySlotUI.item as NodeAbilityInventoryItem
                };
            }
            else
            {
                nodeEquipData = _partNodeUI.CurrentEquipData;
                nodeEquipData.nodeAbilityInventoryItem = nodeAbilitySlotUI.item as NodeAbilityInventoryItem;
            }
            
            SoundPlay();

            _specialPartNodeUI.UpdateNode(nodeEquipData);
            _specialPartNodeUI.skillNode.SkillNodeUpdate();

            _specialPartNodeUI.ShowTransitionGuide(true);
            
            InventoryManager.Instance.RemoveInventoryItemWithSo(nodeAbilitySlotUI.item.data);
        }
    }

    protected override void DropIsNode(PointerEventData eventData)
    {
        _specialPartNodeUI.ShowTransitionGuide(false);
        
        GameObject dragTarget = eventData.pointerDrag;
        if (_specialPartNodeUI.isSpecialMode)
        {
            SpecialPartNodeUI node = dragTarget.GetComponent<SpecialPartNodeUI>();
            if(node == null || node.isAbilityEmpty)
                return;
            
            SoundPlay();
            
            NodeAbilityInventoryItem tempData = node.CurrentEquipData.nodeAbilityInventoryItem;
            node.NodeAbilityChange(_specialPartNodeUI.CurrentEquipData.nodeAbilityInventoryItem);
            _specialPartNodeUI.NodeAbilityChange(tempData);
            
            var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
            dragItem.successDrop = true;
            
            _specialPartNodeUI.ShowTransitionGuide(true);
            _partNodeUI.skillNode.SkillNodeUpdate();
        }
        else
        {
            base.DropIsNode(eventData);
            if(_specialPartNodeUI.isPartEmpty || _specialPartNodeUI.isAbilityEmpty)
                return;
            _specialPartNodeUI.ShowTransitionGuide(true);
        }
    }
}
