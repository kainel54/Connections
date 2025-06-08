using UnityEngine;
using UnityEngine.EventSystems;

public class PartReturnAbleArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        GameObject dragTarget = eventData.pointerDrag;
        SpecialPartNodeUI specialNode = dragTarget.GetComponent<SpecialPartNodeUI>();
        if (specialNode != null && specialNode.isSpecialMode)
            return;            

        PartNodeUI defaultNode = dragTarget.GetComponent<PartNodeUI>();
        if (defaultNode != null)
        {
            var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
            dragItem.successDrop = true;
            defaultNode.ReturnInventoryItem();
            return;
        }
        
        ChainSlotUI chain = dragTarget.GetComponent<ChainSlotUI>();
        if (chain != null)
        {
            chain.ReturnInventory();
        }
    }
}
