public class ProjectileSpeedDownPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectileSpeedUpPart)) is ProjectileSpeedUpPart part)
        {
            part.DecreaseMoveSpeed(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ProjectileSpeedUpPart)) is ProjectileSpeedUpPart part)
        {
            part.IncreaseMoveSpeed(1);
        }
    }
}
