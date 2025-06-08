using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityReturnAbleArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        GameObject dragTarget = eventData.pointerDrag;
        SpecialPartNodeUI node = dragTarget.GetComponent<SpecialPartNodeUI>();
        if (node != null && !node.isSpecialMode)
            return;

        if (node == null)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        dragItem.successDrop = true;
        node.ReturnNodeAbility();
    }
}
