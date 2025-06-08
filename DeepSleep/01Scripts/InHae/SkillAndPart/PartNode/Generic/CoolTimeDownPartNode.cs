public class CoolTimeDownPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimeDownPart)) is CoolTimeDownPart data)
        {
            data.DeCreaseCoolTime(0.5f, ICoolTimeDownPart.ModifyType.Add);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimeDownPart)) is CoolTimeDownPart data)
        {
            data.InCreaseCoolTime(0.5f, ICoolTimeDownPart.ModifyType.Add);
        }
    }
}
