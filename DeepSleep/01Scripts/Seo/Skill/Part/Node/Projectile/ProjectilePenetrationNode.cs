using UnityEngine;

public class ProjectilePenetrationNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectilePenetrationPart)) is ProjectilePenetrationPart part)
        {
            part.SetPenetrationTrue();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectilePenetrationPart)) is ProjectilePenetrationPart part)
        {
            part.SetPenetrationFalse();
        }
    }
}
