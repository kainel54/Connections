using IH.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEquipSlotPointerAction : MonoBehaviour, IDropHandler, IBeginDragHandler, 
    IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private SkillEquipSlot _skillEquipSlot;
    
    private RectTransform _dragTarget;
    private bool _isDragging;
    public bool IsDragging => _isDragging;

    private void Awake()
    {
        _skillEquipSlot = GetComponent<SkillEquipSlot>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(_skillEquipSlot.IsCombatCheck())
            return;
        
        DropItemSlotCase(eventData);
        DropEquipSlotCase(eventData);
    }
    
    private void DropItemSlotCase(PointerEventData eventData)
    {
        // 드래그한 스킬
        GameObject gameObject = eventData.pointerDrag;
        ItemSlotUI slot = gameObject.GetComponent<ItemSlotUI>();

        if(slot == null || slot.isEmpty)
            return;

        if (!_skillEquipSlot.IsEmpty)
            InventoryManager.Instance.AddInventoryItem(ItemType.Skill, _skillEquipSlot.currentSkillItem);

        _skillEquipSlot.UpdateSlot(slot.item as SkillInventoryItem);
        InventoryManager.Instance.RemoveInventoryItem(ItemType.Skill, slot.item);
    }
    
    private void DropEquipSlotCase(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        SkillEquipSlot slot = gameObject.GetComponent<SkillEquipSlot>();

        if (slot == null || slot.IsEmpty)
            return;
        if (slot.CurrentSkill != null && slot.CurrentSkill.IsSkillCoolTime)
            return;
        
        SkillInventoryItem tempData = slot.currentSkillItem;
        slot.UpdateSlot(_skillEquipSlot.currentSkillItem);
        _skillEquipSlot.UpdateSlot(tempData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(_skillEquipSlot.IsEmpty || _skillEquipSlot.IsCombatCheck())
            return;
        
        _isDragging = true;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        dragItem.StartDrag(_skillEquipSlot.currentSkillItem);
        _dragTarget = dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;
        _skillEquipSlot.SetSkillImageColor(Color.clear);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(_skillEquipSlot.IsEmpty || _skillEquipSlot.IsCombat)
            return;
        
        _dragTarget.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left || _skillEquipSlot.IsCombat)
            return;
        
        _isDragging = false;

        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        dragItem.EndDrag();
        _skillEquipSlot.SetSkillImageColor(Color.white);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Right || _skillEquipSlot.IsCombatCheck())
            return;
        
        _skillEquipSlot.Init();
    }
}
