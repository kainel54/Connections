using UnityEngine;
using UnityEngine.EventSystems;

public class SkillReturnAbleAreaUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        GameObject gameObject = eventData.pointerDrag;
        SkillEquipSlot skillEquip = gameObject.GetComponent<SkillEquipSlot>();

        if (skillEquip == null || skillEquip.IsCombat)
            return;
        if(skillEquip.CurrentSkill != null && skillEquip.CurrentSkill.IsSkillCoolTime)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        dragItem.successDrop = true;
        
        skillEquip.Init();
    }
}
