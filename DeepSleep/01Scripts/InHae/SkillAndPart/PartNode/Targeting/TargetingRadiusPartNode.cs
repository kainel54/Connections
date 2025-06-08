using UnityEngine;

public class TargetingRadiusPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(TargetingRadiusUpPart)) is TargetingRadiusUpPart part)
        {
            part.InCreaseTargetingRadius(1);
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(TargetingRadiusUpPart)) is TargetingRadiusUpPart part)
        {
            part.DeCreaseTargetingRadius(1);
        }
    }
}
