using UnityEngine;

public class ProjectileCountUpNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectileCountUpPart)) is ProjectileCountUpPart part)
        {
            part.IncreaseProjectileCount(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectileCountUpPart)) is ProjectileCountUpPart part)
        {
            part.DecreaseProjectileCount(1);
        }
    }
}
