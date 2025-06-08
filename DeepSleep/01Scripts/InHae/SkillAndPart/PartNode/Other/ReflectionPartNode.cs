using UnityEngine;

public class ReflectionPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ReflectionCountUpPart)) is ReflectionCountUpPart part)
        {
            part.ReflectionEquip();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ReflectionCountUpPart)) is ReflectionCountUpPart part)
        {
            part.ReflectionUnEquip();
        }
    }
}
