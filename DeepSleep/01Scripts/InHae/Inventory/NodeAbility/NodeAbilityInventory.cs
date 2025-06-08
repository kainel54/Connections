using UnityEngine;

public class NodeAbilityInventory : Inventory
{
    [SerializeField] private Transform _parent;
    public NodeAbilityStash nodeAbilityStash;
    
    private void Awake()
    {
        nodeAbilityStash = new NodeAbilityStash(_parent);
    }

    public override void AddItemWithSo(ItemDataSO item, int count = 1, int slotIndex = -1)
    {
        bool itemAdded = false;

        if (nodeAbilityStash.CanAddItem(item))
        {
            nodeAbilityStash.AddItemWithSo(item, count, slotIndex);
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
        nodeAbilityStash.RemoveItemWithSo(item, count);
        UpdateSlotUI();
    }

    public override void RemoveItem(InventoryItem item, int count = 1)
    {
    }

    public override bool CanAddItem(ItemDataSO item)
    {
        if (item as NodeAbilityItemSO && nodeAbilityStash.CanAddItem(item))
        {
            return true;
        }
        return false;
    }

    public override Stash GetStash()
    {
        return nodeAbilityStash;
    }

    public override void SetStash(Stash stash)
    {
        nodeAbilityStash = new NodeAbilityStash(_parent, stash);
    }

    public override void UpdateSlotUI()
    {
        nodeAbilityStash.UpdateSlotUI();
    }
}
