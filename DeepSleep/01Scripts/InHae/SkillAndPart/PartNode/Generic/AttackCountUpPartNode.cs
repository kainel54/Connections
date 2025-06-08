using UnityEngine;

public class AttackCountUpPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(AttackCountUpPart)) is AttackCountUpPart part)
        {
            part.InCreaseAttackCount(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(AttackCountUpPart)) is AttackCountUpPart part)
        {
            part.DeCreaseAttackCount(1);
        }
    }
}
