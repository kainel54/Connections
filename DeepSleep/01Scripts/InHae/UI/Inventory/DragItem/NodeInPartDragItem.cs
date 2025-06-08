using UnityEngine;

namespace IH.UI
{
    public class NodeInPartDragItem : DragItem
    {
        [SerializeField] private GameObject _frame;
        
        public override void StartDrag(InventoryItem item)
        {
            base.StartDrag(item);
            gameObject.SetActive(true);
            _inventoryItem = item;
            _itemImage.sprite = item.data.icon;
            _itemImage.color = Color.white;

            successDrop = false;
            _frame.SetActive(true);
        }

        public override void EndDrag()
        {
            base.EndDrag();
            gameObject.SetActive(false);
            _itemImage.color = Color.clear;
            _frame.SetActive(false);
        }
    }
}