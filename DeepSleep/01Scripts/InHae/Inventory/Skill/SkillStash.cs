using UnityEngine;

public class SkillStash : Stash
{
    public SkillStash(Transform parent) : base(parent)
    {
    }

    public SkillStash(Transform parent, Stash copyStash) : base(parent, copyStash)
    {
    }

    public override void AddItemWithSo(ItemDataSO itemData, int count, int slotIndex)
    {
        int idx = slotIndex;
        if (idx < 0)
            idx = FindEmptySlotIndex();
        
        SkillInventoryItem newItem = new SkillInventoryItem(itemData, idx);
        newItem.slotIndex = idx;
        stash.Add(newItem);
        
        SortStashBySlotIndex();
    }
    
    public override void AddItem(InventoryItem item, int count, int slotIndex)
    {
        if(item == null)
            return;
        
        int idx = slotIndex;
        if (idx < 0)
            idx = FindEmptySlotIndex();

        SkillInventoryItem newItem = item as SkillInventoryItem;
        newItem.slotIndex = idx;
        stash.Add(newItem);
        
        SortStashBySlotIndex();
    }

    public override void RemoveItemWithSo(ItemDataSO itemData, int count)
    {
    }
    
    public override void RemoveItem(InventoryItem item, int count)
    {
        if(item == null)
            return;
        
        SkillInventoryItem skillItem = item as SkillInventoryItem;
        stash.Remove(skillItem);
    }

    public override bool CanAddItem(ItemDataSO itemData)
    {
        if (stash.Count >= _itemSlots.Length) 
            return false;

        return true;
    }
}
