using UnityEngine;

public class CoolTimeDownNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimeDownPart)) is CoolTimeDownPart data)
        {
            data.DeCreaseCoolTime(0.5f);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CoolTimeDownPart)) is CoolTimeDownPart data)
        {
            data.InCreaseCoolTime(0.5f);
        }
    }
}
