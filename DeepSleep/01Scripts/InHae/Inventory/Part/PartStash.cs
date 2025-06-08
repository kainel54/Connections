using System;
using System.Collections.Generic;
using UnityEngine;

public class PartStash : Stash
{
    // 계속 리플렉션 안하려고 캐싱용 딕셔너리
    private Dictionary<string, Type> _partNodeTypes = new Dictionary<string, Type>();
    public PartStash(Transform parent) : base(parent)
    {
        
    }

    public PartStash(Transform parent, Stash copyStash) : base(parent, copyStash)
    {
        
    }

    public override void AddItemWithSo(ItemDataSO itemData, int count, int slotIndex)
    {
        if (stashDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack(count);
        }
        else
        {
            int idx = slotIndex;
            if (idx < 0)
                idx = FindEmptySlotIndex();
            
            PartInventoryItem newItem = new PartInventoryItem(itemData, idx);
            PartItemSO partItem = itemData as PartItemSO;
            
            if (!_partNodeTypes.ContainsKey(partItem.nodeScriptName))
                _partNodeTypes.Add(partItem.nodeScriptName, Type.GetType(partItem.nodeScriptName));

            newItem.PartNodeInit(_partNodeTypes[partItem.nodeScriptName]);
            newItem.stackSize = count;
            
            stash.Add(newItem);
            stashDictionary.Add(itemData, newItem);
            
            SortStashBySlotIndex(); //재정렬
        }
    }

    public override void AddItem(InventoryItem item, int count, int slotIndex)
    {
    }

    public override void RemoveItemWithSo(ItemDataSO itemData, int count)
    {
        if (stashDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
        {
            if (inventoryItem.stackSize <= count)
            {
                stash.Remove(inventoryItem);
                stashDictionary.Remove(itemData); //리스트와 딕셔너리 모두에서 제거한다.
            }
            else
            {
                inventoryItem.RemoveStack(count);
            }
        }
    }

    public override void RemoveItem(InventoryItem item, int count)
    {
    }

    public override bool CanAddItem(ItemDataSO itemData)
    {
        if (stashDictionary.ContainsKey(itemData)) 
            return true;

        if (stash.Count >= _itemSlots.Length) 
            return false;

        return true;
    }
}
