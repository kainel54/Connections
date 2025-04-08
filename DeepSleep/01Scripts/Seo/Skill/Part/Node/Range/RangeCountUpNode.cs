using UnityEngine;

public class RangeCountUpNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(RangeCountUpPart)) is RangeCountUpPart part)
        {
            part.IncreaseRangeCount(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(RangeCountUpPart)) is RangeCountUpPart part)
        {
            part.DecreaseRangeCount(1);
        }
    }
}
