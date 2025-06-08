using UnityEngine;

namespace IH.UI
{
    public class PartPopUpPanel : ItemPopUpPanel
    {
        public override void OnPopUp(InventoryItem item)
        {
            base.OnPopUp(item);
            
            gameObject.SetActive(true);
            
            _itemImage.sprite = item.data.icon;
            _itemTitle.text = item.data.itemName;
            _itemDescription.text = item.data.itemDescription;
        }

        public override void EndPopUp()
        {
            base.EndPopUp();
            if(_isFix)
                return;
            gameObject.SetActive(false);
        }
    }
}