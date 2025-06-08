public class BigBigPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CanBigBigPart)) is CanBigBigPart part)
        {
            part.BigbigEquip();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CanBigBigPart)) is CanBigBigPart part)
        {
            part.BigBigUnEquip();
        }
    }
}
