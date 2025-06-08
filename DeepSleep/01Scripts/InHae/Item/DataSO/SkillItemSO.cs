using System.Collections.Generic;
using UnityEngine;

public enum SkillAttackType
{
    Melee, Range
}

[CreateAssetMenu(fileName = "SkillItemSO", menuName = "SO/Item/SkillItemSO")]
public class SkillItemSO : ItemDataSO
{
    private void Awake()
    {
        inventoryType = InventoryType.Skill;
    }

    public List<SkillStatInfoSO> skillStats = new List<SkillStatInfoSO>();
    
    public string reflectionName;
    public GameObject visual;
}
