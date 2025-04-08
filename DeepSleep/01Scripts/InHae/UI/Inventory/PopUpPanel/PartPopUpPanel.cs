namespace IH.UI
{
    public class PartPopUpPanel : ItemPopUpPanel
    {
        private PartInventoryItem _currentItem;

        public override void OnPopUp(InventoryItem item)
        {
            base.OnPopUp(item);
            
            gameObject.SetActive(true);
            
            _currentItem = item as PartInventoryItem;

            _itemImage.sprite = _currentItem.data.icon;
            _itemTitle.text = _currentItem.data.itemName;
            _itemDescription.text = _currentItem.data.itemDescription;
        }

        public override void EndPopUp()
        {
            if(_isFix)
                return;
            gameObject.SetActive(false);
            _currentItem ??= null;
        }
    }
}