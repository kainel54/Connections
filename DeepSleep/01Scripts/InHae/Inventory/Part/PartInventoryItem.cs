using System;
using UnityEngine;

[Serializable]
public class PartInventoryItem : InventoryItem
{
    public PartInventoryItem(ItemDataSO newItemData, int slotIndex, int count = 1) : base(newItemData, slotIndex, count)
    {
    }
    
    public PartNode partNode;

    public void PartNodeInit(Type type)
    {
        if(partNode != null)
            return;
        
        partNode = Activator.CreateInstance(type) as PartNode;
        partNode.partType = ((PartItemSO)data).type;
    }
}
