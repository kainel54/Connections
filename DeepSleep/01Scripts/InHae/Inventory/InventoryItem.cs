using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemDataSO data;
    public int stackSize;
    public int slotIndex;

    public bool IsFullStack => stackSize >= data.maxStack;

    public InventoryItem(ItemDataSO newItemData, int slotIndex, int count = 1)
    {
        data = newItemData;
        this.slotIndex = slotIndex;
        stackSize = count;
    }

    public int AddStack(int count)
    {
        int remainCount = 0;
        stackSize += count;

        if (stackSize > data.maxStack)
        {
            remainCount = stackSize - data.maxStack;
            stackSize = data.maxStack;
        }

        return remainCount;
    }

    public void RemoveStack(int count = 1)
    {
        stackSize -= count;
    }
}
