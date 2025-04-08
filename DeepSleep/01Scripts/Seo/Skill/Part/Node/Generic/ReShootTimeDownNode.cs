using UnityEngine;

public class ReShootTimeDownNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ReShootTimeDownPart)) is ReShootTimeDownPart part)
        {
            part.DecreaseReShootTime(1f);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ReShootTimeDownPart)) is ReShootTimeDownPart part)
        {
            part.IncreaseReShootTime(1f);
        }
    }
}
