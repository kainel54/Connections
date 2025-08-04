using UnityEngine;

public class CriticalChanceUpPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CriticalChanceUpPart)) is CriticalChanceUpPart part)
        {
            part.InCreaseCriticalChance(10);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CriticalChanceUpPart)) is CriticalChanceUpPart part)
        {
            part.DeCreaseCriticalChance(10);
        }
    }
}
