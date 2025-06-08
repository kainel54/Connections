using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropListSO", menuName = "SO/Item/DropListSO")]
public class DropListSO : ScriptableObject
{
    public List<DropItem> itemList;
    public List<SkillItemSO> skillSoList;
    public List<PartItemSO> skillPartsSoList;
    public List<NodeAbilityItemSO> NodeAbilityItemSoList;
    
    public DropItem RandItem() => itemList[Random.Range(0, itemList.Count)];
    public SkillItemSO RandSkill() => skillSoList[Random.Range(0, skillSoList.Count)];
    public PartItemSO RandSkillPart() => skillPartsSoList[Random.Range(0, skillPartsSoList.Count)];
    public NodeAbilityItemSO RandNodeAbility() => NodeAbilityItemSoList[Random.Range(0, NodeAbilityItemSoList.Count)];
}
