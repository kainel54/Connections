using UnityEngine;

public class RangeSizeUpPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(RangeSizeUpPart)) is RangeSizeUpPart part)
        {
            part.InCreaseRangeSize(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(RangeSizeUpPart)) is RangeSizeUpPart part)
        {
            part.DeCreaseRangeSize(1);
        }
    }
}
