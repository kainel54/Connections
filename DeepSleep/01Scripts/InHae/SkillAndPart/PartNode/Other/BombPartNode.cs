public class BombPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(BombPart)) is BombPart part)
        {
            part.BombEquip();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(BombPart)) is BombPart part)
        {
            part.BombUnEquip();
        }
    }

    public override void InitPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(BombPart)) is BombPart part)
        {
            part.InitSetting();
        }
    }
}