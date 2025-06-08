using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeAbilityStash : Stash
{
    // 계속 리플렉션 안하려고 캐싱용 딕셔너리
    private Dictionary<string, Type> _nodeAbilityTypes = new Dictionary<string, Type>();
    public NodeAbilityStash(Transform parent) : base(parent)
    {
        
    }

    public NodeAbilityStash(Transform parent, Stash copyStash) : base(parent, copyStash)
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
            
            NodeAbilityInventoryItem newItem = new NodeAbilityInventoryItem(itemData, idx);
            NodeAbilityItemSO nodeAbilityItem = itemData as NodeAbilityItemSO;
            
            if (!_nodeAbilityTypes.ContainsKey(nodeAbilityItem.reflectionNodeAbilityName))
                _nodeAbilityTypes.Add(nodeAbilityItem.reflectionNodeAbilityName, 
                    Type.GetType(nodeAbilityItem.reflectionNodeAbilityName));

            newItem.NodeAbilityInit(_nodeAbilityTypes[nodeAbilityItem.reflectionNodeAbilityName]);
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
