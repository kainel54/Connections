using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.UIEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class SkillItemSlotUI : ItemSlotUI, IPointerClickHandler
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannelSo;
    
    [SerializeField] private float _yOffset;
    [SerializeField] private GameObject _skillImageObj;
    [SerializeField] private Canvas _canvas;
    
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
        
        _skillImageObj.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        PlaySound();
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
            {
                var equipPartInfoEvt = SkillNodeEvents.EquipPartInfoEvent;
                equipPartInfoEvt.skillInventoryItem = item as SkillInventoryItem;
                _skillNodeEventChannelSo.RaiseEvent(equipPartInfoEvt);
                break;
            }
            case PointerEventData.InputButton.Right:
            {
                var skillAutoEquipEvt = SkillNodeEvents.SkillAutoEquipEvent;
                skillAutoEquipEvt.skillInventoryItem = item as SkillInventoryItem;
                _skillNodeEventChannelSo.RaiseEvent(skillAutoEquipEvt);

                var slotSelectActiveEvt = UIEvents.ItemSlotSelectActiveEvent;
                slotSelectActiveEvt.isActive = false;
                _uiEventChannel.RaiseEvent(slotSelectActiveEvt);
                break;
            }
        }
    }
}
