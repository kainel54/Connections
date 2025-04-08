using UnityEngine;

public class PartInventory : Inventory
{
    [SerializeField] private Transform _parent;
    public PartStash partStash;
    
    private void Awake()
    {
        partStash = new PartStash(_parent);
    }

    public override void AddItemWithSo(ItemDataSO item, int count = 1, int slotIndex = -1)
    {
        bool itemAdded = false;

        if (partStash.CanAddItem(item))
        {
            partStash.AddItemWithSo(item, count, slotIndex);
            itemAdded = true;
        }
        
        if (itemAdded)
        {
            UpdateSlotUI();
        }
    }

    public override void AddItem(InventoryItem item, int count = 1, int slotIndex = -1)
    {
    }

    public override void RemoveItemWithSo(ItemDataSO item, int count = 1)
    {
        partStash.RemoveItemWithSo(item, count);
        UpdateSlotUI();
    }

    public override void RemoveItem(InventoryItem item, int count = 1)
    {
    }

    public override bool CanAddItem(ItemDataSO item)
    {
        if (item as PartItemSO && partStash.CanAddItem(item))
        {
            return true;
        }
        return false;
    }

    public override Stash GetStash()
    {
        return partStash;
    }

    public override void SetStash(Stash stash)
    {
        partStash = new PartStash(_parent, stash);
    }

    public override void UpdateSlotUI()
    {
        partStash.UpdateSlotUI();
    }
}
