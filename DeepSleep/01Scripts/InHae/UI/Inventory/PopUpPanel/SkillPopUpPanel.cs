using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IH.UI
{
    public class SkillPopUpPanel : ItemPopUpPanel
    {
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private Transform _partListParent;
        private SkillInventoryItem _currentItem;

        private List<SkillPopUpPartSlot> _skillPopUpPartSlots;

        protected override void Awake()
        {
            base.Awake();
            _skillPopUpPartSlots = _partListParent.GetComponentsInChildren<SkillPopUpPartSlot>().ToList();
        }

        public override void OnPopUp(InventoryItem item)
        {
            base.OnPopUp(item);
            _scrollbar.value = 1;
            
            gameObject.SetActive(true);
            
            _currentItem = item as SkillInventoryItem;

            _itemImage.sprite = _currentItem.data.icon;
            _itemTitle.text = _currentItem.data.itemName;
            _itemDescription.text = _currentItem.data.itemDescription;

            int i = 0;
            foreach (var equipPart in _currentItem.equipNodeData.Values)
            {
                if(equipPart.partInventoryItem == null || equipPart.partInventoryItem.data == null)
                    continue;
                
                _skillPopUpPartSlots[i].UpdateSlot(equipPart.partInventoryItem);
                i++;
            }
        }

        public override void EndPopUp()
        {
            if(_isFix)
                return;
            int i = 0;
            gameObject.SetActive(false);

            if (_currentItem == null)
                return;
            
            foreach (var equipPart in _currentItem.equipNodeData.Values)
            {
                if(equipPart.partInventoryItem == null || equipPart.partInventoryItem.data == null)
                    continue;
                
                _skillPopUpPartSlots[i].CleanUpSlot();
                i++;
            }
            _currentItem = null;
        }
    }
}