using System.Collections.Generic;
using UnityEngine;

public abstract class Stash
{
    public List<InventoryItem> stash;
    public Dictionary<ItemDataSO, InventoryItem> stashDictionary; //검색을 편하게 하기 위해 2중으로 유지
    protected Transform _slotParent;
    protected ItemSlotUI[] _itemSlots;

    public Stash(Transform parent)
    {
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemDataSO, InventoryItem>();
        _slotParent = parent;
        _itemSlots = parent.GetComponentsInChildren<ItemSlotUI>();
        
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].slotIndex = i;
        }
    }

    public Stash(Transform parent, Stash copyStash)
    {
        if(copyStash == null)
            return;
        
        stash = copyStash.stash;
        stashDictionary = copyStash.stashDictionary;
        _slotParent = parent;
        _itemSlots = parent.GetComponentsInChildren<ItemSlotUI>();
        
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].slotIndex = i;
        }
    }

    public virtual void UpdateSlotUI()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stash.Count; i++)
        {
            _itemSlots[stash[i].slotIndex].UpdateSlot(stash[i]);
        }
    }

    protected int FindEmptySlotIndex()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            InventoryItem item = stash.Find(x => x.slotIndex == i);
            if (item == null) return i;
        }

        return -1; //비어있는 슬롯이 없을 경우.
    }

    protected virtual void SortStashBySlotIndex()
    {
        //슬롯 인덱스의 오름차순 정렬
        stash.Sort((a,b)=>a.slotIndex-b.slotIndex);
    }

    public virtual bool HasItem(ItemDataSO itemData) => stashDictionary.ContainsKey(itemData);

    public abstract void AddItemWithSo(ItemDataSO itemData, int count, int slotIndex);
    public abstract void AddItem(InventoryItem item, int count, int slotIndex);
    public abstract void RemoveItemWithSo(ItemDataSO itemData, int count);
    public abstract void RemoveItem(InventoryItem item, int count);
    public abstract bool CanAddItem(ItemDataSO itemData);
}
