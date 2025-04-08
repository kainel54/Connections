using UnityEngine;

public class SkillActiveDurationNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ActiveDurationUpPart)) is ActiveDurationUpPart part)
        {
            part.InCreaseActiveDuration(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ActiveDurationUpPart)) is ActiveDurationUpPart part)
        {
            part.DeCreaseActiveDuration(1);
        }
    }
}
