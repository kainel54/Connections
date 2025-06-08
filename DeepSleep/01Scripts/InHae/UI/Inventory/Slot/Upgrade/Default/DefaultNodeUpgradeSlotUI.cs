using System;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YH.EventSystem;

public class DefaultNodeUpgradeSlotUI : ItemSlotUI, IPointerClickHandler
{
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannelSO;
    
    [SerializeField] private float _xOffset;
    [SerializeField] private GameObject _skillImageObj;
    
    private bool _isLocked;

    private void Awake()
    {
        _defaultNodeEventChannelSO.AddListener<UpgradeSkillSelectLockEvent>(HandleLockChange);
    }

    private void OnDestroy()
    {
        _defaultNodeEventChannelSO.RemoveListener<UpgradeSkillSelectLockEvent>(HandleLockChange);
    }
    
    private void HandleLockChange(UpgradeSkillSelectLockEvent evt)
    {
        _isLocked = evt.isLocked;
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
  
        // var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Skill);
        // Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        //
        // RectTransform popUpRect = popUp.transform as RectTransform;
        // Vector2 popUpSize = popUpRect.sizeDelta;
        //
        // pos.x -= popUpSize.x * 0.5f + _xOffset;
        //
        // float halfHeight = popUpSize.y * 0.5f;
        // pos.y = Mathf.Clamp(pos.y, halfHeight, Screen.height - halfHeight);
        //
        // popUpRect.position = pos;
        // popUp.OnPopUp(item);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        
        if(isEmpty)
            return;
        
        // var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Skill);
        // popUp.EndPopUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isEmpty || _isLocked)
            return;
        PlaySound();

        var selectImageEvt = DefaultNodeUpgradeEvents.UpgradeSlotSelectImageEvent;
        selectImageEvt.targetTrm = transform as RectTransform;
        _defaultNodeEventChannelSO.RaiseEvent(selectImageEvt);
        
        var skillSelectEvt = DefaultNodeUpgradeEvents.UpgradeSkillSelectEvent;
        skillSelectEvt.item = item as SkillInventoryItem;
        _defaultNodeEventChannelSO.RaiseEvent(skillSelectEvt);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if(isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;
        
        _skillImageObj.SetActive(false);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if(isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;
        
        _skillImageObj.SetActive(true);
    }
}
