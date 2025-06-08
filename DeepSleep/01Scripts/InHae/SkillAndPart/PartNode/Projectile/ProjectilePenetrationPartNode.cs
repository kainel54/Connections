using UnityEngine;

public class ProjectilePenetrationPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectilePenetrationCountUpPart)) is ProjectilePenetrationCountUpPart part)
        {
            part.SetPenetrationCountUp();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectilePenetrationCountUpPart)) is ProjectilePenetrationCountUpPart part)
        {
            part.SetPenetrationCountDown();
        }
    }
}
