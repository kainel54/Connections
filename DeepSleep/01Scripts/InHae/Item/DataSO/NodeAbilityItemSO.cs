using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeAbilityItemSO", menuName = "SO/Item/NodeAbilityItemSO")]
public class NodeAbilityItemSO : ItemDataSO
{
    private void Awake()
    {
        inventoryType = InventoryType.NodeAbility;
    }

    public string reflectionNodeAbilityName;
    public GameObject visual;
}
