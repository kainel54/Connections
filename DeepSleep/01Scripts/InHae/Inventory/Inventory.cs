using UnityEngine;

public enum InventoryType
{
    //인벤토리에 들어가지 않는 아이템들
    None,
    Skill,
    Part,
    NodeAbility,
}

public abstract class Inventory : MonoBehaviour
{
    public InventoryType type;
    public abstract void AddItemWithSo(ItemDataSO item, int count = 1, int slotIndex = -1);
    public abstract void AddItem(InventoryItem item, int count = 1, int slotIndex = -1);
    public abstract void RemoveItemWithSo(ItemDataSO item, int count = 1);
    public abstract void RemoveItem(InventoryItem item, int count = 1);
    public abstract bool CanAddItem(ItemDataSO item);
    public abstract Stash GetStash();
    
    public abstract void SetStash(Stash stash);
    public abstract void UpdateSlotUI();
}
