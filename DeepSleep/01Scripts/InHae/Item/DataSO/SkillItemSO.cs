using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum SkillAttackType
{
    Melee, Range
}

[CreateAssetMenu(fileName = "SkillItemSO", menuName = "SO/Item/SkillItemSO")]
public class SkillItemSO : ItemDataSO
{
    private void Awake()
    {
        itemType = ItemType.Skill;
    }

    [FormerlySerializedAs("skillStats")]
    public List<SkillStatInfoSO> descriptionUseSkillStat = new List<SkillStatInfoSO>();
    
    public string reflectionName;
    public GameObject visual;
}
