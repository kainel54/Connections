using IH.EventSystem.SoundEvent;
using IH.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class PartNodeUIDrop : MonoBehaviour, IDropHandler, IPartNodeUIComponent
{
    protected PartNodeUI _partNodeUI;
    private PartNodeUIChainCheck _partNodeUIChainCheck;
    
    protected PartNodeUIOnPopUp _partNodeUIOnPopUp;
    
    [SerializeField] private GameEventChannelSO _soundChannelSO;
    [SerializeField] private SoundSO _equipPartSound;

    public virtual void Initialize(PartNodeUI partNodeUI)
    {
        _partNodeUI = partNodeUI;
        _partNodeUIChainCheck = _partNodeUI.GetCompo<PartNodeUIChainCheck>();
        _partNodeUIOnPopUp = _partNodeUI.GetCompo<PartNodeUIOnPopUp>();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if(_partNodeUI.isSpecialMode)
            return;
        
        GameObject dragTarget = eventData.pointerDrag;
        if(dragTarget == gameObject)
            return;
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        if (_partNodeUIChainCheck.isChainMode)
        {
            DropIsChainMode(eventData);
            return;
        }
        
        // 인벤에서 노드로 오는 경우
        DropPartIsInventory(eventData);
        // 노드에서 노드로 오는 경우 
        DropIsNode(eventData);
    }

    private void DropIsChainMode(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        PartItemSlotUI partSlotUI = gameObject.GetComponent<PartItemSlotUI>();

        if (partSlotUI == null || partSlotUI.isEmpty || !_partNodeUIChainCheck.isChainAble)
            return;
        
        SoundPlay();

        _partNodeUIChainCheck.AddChainItem(partSlotUI.item as PartInventoryItem);
        _partNodeUIChainCheck.FindChainNode();

        InventoryManager.Instance.RemoveInventoryItemWithSo(partSlotUI.item.data);
    }

    protected virtual void DropIsNode(PointerEventData eventData)
    {
        GameObject dragTarget = eventData.pointerDrag;
        
        // 드래그를 시작한 노드
        PartNodeUI node = dragTarget.GetComponent<PartNodeUI>();
        if (node == null)
            return;
        if(node is SpecialPartNodeUI specialNodeUI && specialNodeUI.isSpecialMode)
            return;
        if(node.isPartEmpty)
            return;
        
        SoundPlay();
        
        PartInventoryItem tempData = node.CurrentEquipData.partInventoryItem;
        node.UnEquipCurrentPart();
        node.NodePartChange(_partNodeUI.CurrentEquipData.partInventoryItem);
        
        _partNodeUI.UnEquipCurrentPart();
        _partNodeUI.NodePartChange(tempData);
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        dragItem.successDrop = true;

        var chainPopUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Chain);
        chainPopUp.EndPopUp();
        
        var partPopUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
        _partNodeUIOnPopUp.SetPopUpOn(partPopUp, _partNodeUI.CurrentEquipData.partInventoryItem);
        
        _partNodeUI.skillNode.SkillNodeUpdate();
    }

    private void DropPartIsInventory(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        PartItemSlotUI partSlotUI = gameObject.GetComponent<PartItemSlotUI>();
        if (partSlotUI != null && !partSlotUI.isEmpty)
        {
            if (!_partNodeUI.isPartEmpty)
            {
                InventoryManager.Instance.AddInventoryItemWithSo(_partNodeUI.CurrentEquipData.partInventoryItem.data);
                _partNodeUI.UnEquipCurrentPart();
            }

            NodeEquipData nodeEquipData;
            if (_partNodeUI.CurrentEquipData == null)
            {
                nodeEquipData = new NodeEquipData
                {
                    partInventoryItem = partSlotUI.item as PartInventoryItem
                };
            }
            else
            {
                nodeEquipData = _partNodeUI.CurrentEquipData;
                nodeEquipData.partInventoryItem = partSlotUI.item as PartInventoryItem;
            }
            SoundPlay();

            _partNodeUI.UpdateNode(nodeEquipData);
            _partNodeUI.skillNode.SkillNodeUpdate();
            
            var partPopUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
            _partNodeUIOnPopUp.SetPopUpOn(partPopUp, _partNodeUI.CurrentEquipData.partInventoryItem);
            
            InventoryManager.Instance.RemoveInventoryItemWithSo(partSlotUI.item.data);
        }
    }

    protected void SoundPlay()
    {
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _equipPartSound;
        soundPlayEvt.position = transform.position;
        _soundChannelSO.RaiseEvent(soundPlayEvt);
    }
}
