using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.EventSystem.UIEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class NodeAbilitySlotUI : ItemSlotUI, IPointerClickHandler
{
    [SerializeField] private GameEventChannelSO _specialPartNodeEventChannel;
    [SerializeField] private GameObject _skillImageObj;
    [SerializeField] private float _xOffset;

    private ItemPopUpPanel _nodeAbilityPopUpPanel;

    private void Start()
    {
        _nodeAbilityPopUpPanel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.NodeAbility);
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
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        
        if(isEmpty)
            return;
        
        var drag = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        if(drag.isDragging)
            return;
        
        RectTransform popUpRect = _nodeAbilityPopUpPanel.transform as RectTransform;
        Vector2 popUpSize = popUpRect.sizeDelta;
        
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x -= popUpSize.x * 0.5f + _xOffset;
        popUpRect.position = pos;
        
        _nodeAbilityPopUpPanel.OnPopUp(item);
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if(isEmpty)
            return;
        
        _nodeAbilityPopUpPanel.EndPopUp();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if(isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;
        
        _skillImageObj.SetActive(false);
        _nodeAbilityPopUpPanel.EndPopUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isEmpty)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var partAutoEquipEvt = SpecialPartNodeEvents.AutoEquipAbilityEvent;
            partAutoEquipEvt.ability = item as NodeAbilityInventoryItem;
            _specialPartNodeEventChannel.RaiseEvent(partAutoEquipEvt);
            
            var slotSelectActiveEvt = UIEvents.ItemSlotSelectActiveEvent;
            slotSelectActiveEvt.isActive = false;
            _uiEventChannel.RaiseEvent(slotSelectActiveEvt);
            
            _nodeAbilityPopUpPanel.EndPopUp();
        }
    }
}
