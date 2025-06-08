using UnityEngine;

public class DamageUpPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(DamageUpPart)) is DamageUpPart part)
        {
            part.InCreaseDamage(10);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(DamageUpPart)) is DamageUpPart part)
        {
            part.DeCreaseDamage(10);
        }
    }
}
