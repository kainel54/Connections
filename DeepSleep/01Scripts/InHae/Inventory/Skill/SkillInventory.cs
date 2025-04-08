using UnityEngine;

public class SkillInventory : Inventory
{
    [SerializeField] private Transform _parent;
    public SkillStash skillStash;
    
    private void Awake()
    {
        skillStash = new SkillStash(_parent);
    }

    public override void AddItemWithSo(ItemDataSO item, int count = 1, int slotIndex = -1)
    {
        bool itemAdded = false;

        if (skillStash.CanAddItem(item))
        {
            skillStash.AddItemWithSo(item, count, slotIndex);
            itemAdded = true;
        }
        
        if (itemAdded)
        {
            UpdateSlotUI();
        }
    }

    public override void AddItem(InventoryItem item, int count = 1, int slotIndex = -1)
    {
        bool itemAdded = false;

        if (skillStash.CanAddItem(null))
        {
            skillStash.AddItem(item, count, slotIndex);
            itemAdded = true;
        }
        
        if (itemAdded)
        {
            UpdateSlotUI();
        }
    }

    public override void RemoveItemWithSo(ItemDataSO item, int count = 1)
    {
        skillStash.RemoveItemWithSo(item, count);
        UpdateSlotUI();
    }

    public override void RemoveItem(InventoryItem item, int count = 1)
    {
        skillStash.RemoveItem(item, count);
        UpdateSlotUI();
    }

    public override bool CanAddItem(ItemDataSO item)
    {
        if (item as SkillItemSO && skillStash.CanAddItem(item))
        {
            return true;
        }
        return false;
    }

    public override Stash GetStash()
    {
        return skillStash;
    }

    public override void SetStash(Stash stash)
    {
        skillStash = new SkillStash(_parent, stash);
        UpdateSlotUI();
    }

    public override void UpdateSlotUI()
    {
        skillStash.UpdateSlotUI();
    }
}
