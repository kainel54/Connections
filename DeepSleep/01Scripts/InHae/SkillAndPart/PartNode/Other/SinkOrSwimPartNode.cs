public class SinkOrSwimPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimeDownPart)) is CoolTimeDownPart part)
        {
            skill.UseSkillAction += Action;
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimeDownPart)) is CoolTimeDownPart part)
        {
            skill.UseSkillAction -= Action;
        }
    }

    public void Action(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimeDownPart)) is CoolTimeDownPart part)
        {
            part.SetCoolTimeBetweenTwo(2, 10);
        }
    }
}
