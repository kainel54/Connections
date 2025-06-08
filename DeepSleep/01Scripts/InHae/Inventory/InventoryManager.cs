using System.Collections.Generic;
using System.Linq;

namespace IH.Manager
{
    public class InventoryManager : MonoSingleton<InventoryManager>
    {
        private Dictionary<InventoryType, Inventory> _inventoryDictionary = new Dictionary<InventoryType, Inventory>();

        private void Awake()
        {
            GetComponentsInChildren<Inventory>().ToList().ForEach(x=> _inventoryDictionary.Add(x.type, x));
        }

        public void AddInventoryItemWithSo(ItemDataSO itemData) => 
            _inventoryDictionary[itemData.inventoryType].AddItemWithSo(itemData);
        
        public void AddInventoryItem(InventoryType type, InventoryItem item) => 
            _inventoryDictionary[type].AddItem(item);

        public void RemoveInventoryItemWithSo(ItemDataSO itemData) => 
            _inventoryDictionary[itemData.inventoryType].RemoveItemWithSo(itemData);
        
        public void RemoveInventoryItem(InventoryType type, InventoryItem item) => 
            _inventoryDictionary[type].RemoveItem(item);
        
        public bool CanAddItem(ItemDataSO itemData) => 
            _inventoryDictionary[itemData.inventoryType].CanAddItem(itemData);
        
        public Inventory GetInventory(InventoryType type) => 
            _inventoryDictionary[type];
        
        public void SetStash(InventoryType type, Stash stash) => 
            _inventoryDictionary[type].SetStash(stash);
    }
}
