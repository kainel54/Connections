using UnityEngine;

public class TargetingRadiusPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(TargetingRadiusPart)) is TargetingRadiusPart part)
        {
            part.InCreaseTargetingRadius(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(TargetingRadiusPart)) is TargetingRadiusPart part)
        {
            part.DeCreaseTargetingRadius(1);
        }
    }
}
