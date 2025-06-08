using IH.EventSystem.NodeEvent.NodeChainEvent;
using IH.EventSystem.NodeEvent.PartNodeEvents;
using IH.EventSystem.UIEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class PartItemSlotUI : ItemSlotUI, IPointerClickHandler
{
    [SerializeField] private GameEventChannelSO _nodeChainEventChannel;
    [SerializeField] private GameEventChannelSO _partNodeEventChannel;
    
    [SerializeField] private GameObject _skillImageObj;
    [SerializeField] private float _xOffset;

    private bool _isChainMode;
    
    private ItemPopUpPanel _partPopUpPanel;

    private void Awake()
    {
        _nodeChainEventChannel.AddListener<ChainModeChangeEvent>(HandleChainModeChangeEvent);
    }

    private void Start()
    {
        _partPopUpPanel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
    }

    private void OnDestroy()
    {
        _nodeChainEventChannel.RemoveListener<ChainModeChangeEvent>(HandleChainModeChangeEvent);
    }

    private void HandleChainModeChangeEvent(ChainModeChangeEvent evt)
    {
        _isChainMode = evt.isActive;
    }

    public override void UpdateSlot(InventoryItem newItem)
    {
        _skillImageObj.SetActive(true);
        item = newItem;
        _itemImage.color = Color.white;
        
        if(!isEmpty)
        {
            _itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
                _amountText.text = item.stackSize.ToString(); 
            else
                _amountText.text = string.Empty;
            
            item.slotIndex = slotIndex;
        }
        else
        {
            CleanUpSlot();
        }
    }

    public override void CleanUpSlot()
    {
        base.CleanUpSlot();
        _skillImageObj.SetActive(false);
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        _partPopUpPanel.SetFix(false);
        _partPopUpPanel.EndPopUp();
        _skillImageObj.SetActive(false);

        ChainAbleEvent();
    }

    private void ChainAbleEvent()
    {
        if(!_isChainMode)
            return;

        var evt = NodeChainEvents.ChainPartSelectEvent;
        evt.partItemSO = item.data as PartItemSO;
        _nodeChainEventChannel.RaiseEvent(evt);
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        
        if(isEmpty)
            return;
        
        var drag = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        if(drag.isDragging)
            return;
        
        RectTransform popUpRect = _partPopUpPanel.transform as RectTransform;
        Vector2 popUpSize = popUpRect.sizeDelta;
        
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x -= popUpSize.x * 0.5f + _xOffset;
        popUpRect.position = pos;
        
        _partPopUpPanel.OnPopUp(item);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if(isEmpty)
            return;
        
        _partPopUpPanel.EndPopUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isEmpty)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var partAutoEquipEvt = PartNodeEvent.AutoEquipPartEvent;
            partAutoEquipEvt.part = item as PartInventoryItem;
            _partNodeEventChannel.RaiseEvent(partAutoEquipEvt);
            
            var slotSelectActiveEvt = UIEvents.ItemSlotSelectActiveEvent;
            slotSelectActiveEvt.isActive = false;
            _uiEventChannel.RaiseEvent(slotSelectActiveEvt);
            
            _partPopUpPanel.EndPopUp();
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        var evt = NodeChainEvents.ChainPartSelectCompleteEvent;
        _nodeChainEventChannel.RaiseEvent(evt);
    }
}
