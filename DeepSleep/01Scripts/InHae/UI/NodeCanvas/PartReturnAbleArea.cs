using UnityEngine;
using UnityEngine.EventSystems;

public class PartReturnAbleArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        GameObject gameObject = eventData.pointerDrag;
        PartNodeUI node = gameObject.GetComponent<PartNodeUI>();

        if (node != null)
        {
            var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
            dragItem.successDrop = true;
            node.ReturnInventoryItem();
            return;
        }
        
        ChainSlotUI chain = gameObject.GetComponent<ChainSlotUI>();
        if (chain != null)
        {
            chain.ReturnInven();
        }
    }
}
