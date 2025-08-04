using UnityEngine;

public class LightningPartNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if(skill.GetSkillPart(typeof(LightningPart)) is LightningPart part)
        {
            part.LightningEquip();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(LightningPart)) is LightningPart part)
        {
            part.LightningUnEquip();
        }
    }
}
