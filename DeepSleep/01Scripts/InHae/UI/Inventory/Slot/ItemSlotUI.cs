using System;
using IH.EventSystem.SoundEvent;
using IH.EventSystem.UIEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YH.EventSystem;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected GameEventChannelSO _uiEventChannel;
    [SerializeField] private GameEventChannelSO _soundChannelSo;
    [SerializeField] private SoundSO _clickSound;
    
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _amountText;
    [HideInInspector] public int slotIndex;
        
    public InventoryItem item;
    public RectTransform RectTrm => transform as RectTransform;
    protected RectTransform _dragTarget;

    protected bool _isDropable = true;
    public bool isEmpty => item == null || item.data == null;

    public virtual void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        _itemImage.color = Color.white;
        if (!isEmpty)
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

    public virtual void CleanUpSlot()
    {
        item = null;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _amountText.text = string.Empty;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        dragItem.StartDrag(item);
        _dragTarget = dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;
        
        _itemImage.color =Color.clear;
        _amountText.text = string.Empty;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if(isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;
        
        _dragTarget.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        if (!dragItem.successDrop)
        {
            UpdateSlot(item);
        }
        dragItem.EndDrag();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        GameObject gameObject = eventData.pointerDrag;
        ItemSlotUI slot = gameObject.GetComponent<ItemSlotUI>();

        if(slot == null || slot.isEmpty ||!slot._isDropable)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        InventoryItem item = this.item;

        UpdateSlot(slot.item);
        slot.UpdateSlot(item);
        
        dragItem.successDrop = true;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        
        var itemSlotActiveEvent = UIEvents.ItemSlotSelectActiveEvent;
        itemSlotActiveEvent.isActive = true;
        _uiEventChannel.RaiseEvent(itemSlotActiveEvent);
        
        var itemSlotSelectEvent = UIEvents.ItemSlotSelectEvent;
        itemSlotSelectEvent.targetTrm = transform as RectTransform;
        _uiEventChannel.RaiseEvent(itemSlotSelectEvent);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        
        var itemSlotActiveEvent = UIEvents.ItemSlotSelectActiveEvent;
        itemSlotActiveEvent.isActive = false;
        _uiEventChannel.RaiseEvent(itemSlotActiveEvent);
    }

    protected void PlaySound()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.clipData = _clickSound;
        soundEvt.position = transform.position;
        _soundChannelSo.RaiseEvent(soundEvt);
    }
}
