public class SinkOrSwimPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimePart)) is CoolTimePart part)
        {
            skill.UseSkillAction += Action;
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimePart)) is CoolTimePart part)
        {
            skill.UseSkillAction -= Action;
        }
    }

    public void Action(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimePart)) is CoolTimePart part)
        {
            part.SetCoolTimeBetweenTwo(2, 10);
        }
    }
}
