using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeAbilityItemSO", menuName = "SO/Item/NodeAbilityItemSO")]
public class NodeAbilityItemSO : ItemDataSO
{
    private void Awake()
    {
        itemType = ItemType.NodeAbility;
    }

    public string reflectionNodeAbilityName;
    public GameObject visual;
}
