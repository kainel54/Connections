using System;
using System.Collections.Generic;
using IH.UI;
using UnityEngine;
using UnityEngine.Serialization;

public enum NodeDir
{
    Left = 0,
    LeftTop,
    RightTop,
    Right,
    RightBottom,
    LeftBottom,
}

[Serializable]
public class NodeEquipData
{
    public NodeAbilityInventoryItem nodeAbilityInventoryItem;
    public PartInventoryItem partInventoryItem;
    public List<PartInventoryItem> chainList = new List<PartInventoryItem>();
}

[Serializable]
public struct NodeData
{
    public NodeData(bool isSpecial, int idx, int dis, Vector2Int grid)
    {
        this.isSpecial = isSpecial;
        index = idx;
        distance = dis;
        connectNodeGridList = new List<Vector2Int>();
        this.grid = grid;
    }
    
    public bool isSpecial;
    public int index;
    public int distance;
    public List<Vector2Int> connectNodeGridList;
    public Vector2Int grid;
}

[Serializable]
public class SkillInventoryItem : InventoryItem
{
    public Dictionary<int, NodeEquipData> equipNodeData;
    public Dictionary<Vector2Int, NodeData> nodeGridDictionary;

    public SkillInventoryItem(ItemDataSO newItemData, int slotIndex, int count = 1) : base(newItemData, slotIndex, count)
    {
        nodeGridDictionary = new Dictionary<Vector2Int, NodeData>();
        for (int i = 0; i < 6; i++)
        {
            Vector2Int grid = Vector2Int.zero;
            grid += NodeModular.GetNodeDirGrid((NodeDir)i);
            
            NodeData nodeData = new NodeData(false, i, 1, grid);
            nodeGridDictionary.Add(grid, nodeData);
        }
        
        equipNodeData = new Dictionary<int, NodeEquipData>();
    }
}
