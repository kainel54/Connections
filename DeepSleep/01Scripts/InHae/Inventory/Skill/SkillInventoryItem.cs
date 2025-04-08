using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeData
{
    public NodeData()
    {
        
    }
    
    public NodeData(NodeData nodeData)
    {
        if (nodeData == null)
        {
            partInventoryItem = null;
            chainList.Clear();
            return;
        }
        
        partInventoryItem = nodeData.partInventoryItem;
        chainList = nodeData.chainList;
    }
    
    public PartInventoryItem partInventoryItem;
    public List<PartInventoryItem> chainList = new List<PartInventoryItem>();
}

[Serializable]
public class SkillInventoryItem : InventoryItem
{
    public Dictionary<int, NodeData> equipNodeData;
    public List<int> rowAndNodeCountList;

    public SkillInventoryItem(ItemDataSO newItemData, int slotIndex, int count = 1) : base(newItemData, slotIndex, count)
    {
        rowAndNodeCountList = (newItemData as SkillItemSO).defaultNodeCount;
        equipNodeData = new Dictionary<int, NodeData>();
    }
}
