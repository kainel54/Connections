public class CoolTimeDownPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimePart)) is CoolTimePart data)
        {
            data.DeCreaseCoolTime(0.5f, ICoolTimePart.ModifyType.Add);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimePart)) is CoolTimePart data)
        {
            data.InCreaseCoolTime(0.5f, ICoolTimePart.ModifyType.Add);
        }
    }
}
