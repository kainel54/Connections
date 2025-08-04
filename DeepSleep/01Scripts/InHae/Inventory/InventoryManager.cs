 using System.Collections.Generic;
using System.Linq;

namespace IH.Manager
{
    public class InventoryManager : MonoSingleton<InventoryManager>
    {
        private Dictionary<ItemType, Inventory> _inventoryDictionary = new Dictionary<ItemType, Inventory>();

        private void Awake()
        {
            GetComponentsInChildren<Inventory>().ToList().ForEach(x=> _inventoryDictionary.Add(x.type, x));
        }

        public void AddInventoryItemWithSo(ItemDataSO itemData) => 
            _inventoryDictionary[itemData.itemType].AddItemWithSo(itemData);
        
        public void AddInventoryItem(ItemType type, InventoryItem item) => 
            _inventoryDictionary[type].AddItem(item);

        public void RemoveInventoryItemWithSo(ItemDataSO itemData) => 
            _inventoryDictionary[itemData.itemType].RemoveItemWithSo(itemData);
        
        public void RemoveInventoryItem(ItemType type, InventoryItem item) => 
            _inventoryDictionary[type].RemoveItem(item);
        
        public bool CanAddItem(ItemDataSO itemData) => 
            _inventoryDictionary[itemData.itemType].CanAddItem(itemData);
        
        public Inventory GetInventory(ItemType type) => 
            _inventoryDictionary[type];
        
        public void SetStash(ItemType type, Stash stash) => 
            _inventoryDictionary[type].SetStash(stash);
    }
}
