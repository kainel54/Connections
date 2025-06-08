using IH.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChainSlotUI : ItemSlotUI
{  
    [SerializeField] private GameObject _skillImageObj;
    private PartNodeUIChainCheck _currentNodeUI;
    
    private void Awake()
    {
        _isDropable = false;
    }

    public void Init(PartNodeUIChainCheck uiChain)
    {
        if(isEmpty)
            return;
        
        _currentNodeUI = uiChain;
    }

    public override void UpdateSlot(InventoryItem newItem)
    {
        _skillImageObj.SetActive(true);
        item = newItem;
        _itemImage.color = Color.white;

        if(!isEmpty)
        {
            _itemImage.sprite = item.data.icon;
        }
        else
        {
            CleanUpSlot();
        }
    }

    public override void CleanUpSlot()
    {
        _skillImageObj.SetActive(false);
        base.CleanUpSlot();
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        dragItem.StartDrag(item);
        _dragTarget = dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;
        
        _itemImage.color =Color.clear;
        _amountText.text = string.Empty;

        _skillImageObj.gameObject.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        if (!dragItem.successDrop)
            UpdateSlot(item);
        
        dragItem.EndDrag();
    }

    public override void OnDrop(PointerEventData eventData)
    {
    }

    public void ReturnInventory()
    {
        if(isEmpty)
            return;
        
        InventoryManager.Instance.AddInventoryItemWithSo(item.data);
        _currentNodeUI.RemoveChainPart(item as PartInventoryItem);
        item = null;
    }
}
