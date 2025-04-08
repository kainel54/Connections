using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
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
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.SkillAndPart);
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
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.SkillAndPart);
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

        if(slot == null || !slot._isDropable)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.SkillAndPart);
        InventoryItem item = this.item;

        UpdateSlot(slot.item);
        slot.UpdateSlot(item);
        
        dragItem.successDrop = true;
    }
}
