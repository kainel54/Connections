using System;

[Serializable]
public class NodeAbilityInventoryItem : InventoryItem
{
    public NodeAbilityInventoryItem(ItemDataSO newItemData, int slotIndex, int count = 1) : base(newItemData, slotIndex, count)
    {
    }
    
    public BaseNodeAbility nodeAbility;

    public void NodeAbilityInit(Type type)
    {
        if(nodeAbility != null)
            return;
        
        nodeAbility = Activator.CreateInstance(type) as BaseNodeAbility;
    }
}
