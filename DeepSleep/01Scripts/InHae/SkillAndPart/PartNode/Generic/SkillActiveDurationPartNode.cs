using UnityEngine;

public class SkillActiveDurationPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ActiveDurationIncreasePart)) is ActiveDurationIncreasePart part)
        {
            part.InCreaseActiveDuration(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ActiveDurationIncreasePart)) is ActiveDurationIncreasePart part)
        {
            part.DeCreaseActiveDuration(1);
        }
    }
}
