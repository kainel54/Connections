using UnityEngine;

public class CircleTrajectoryNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CircleTrajectoryPart)) is CircleTrajectoryPart part)
        {
            part.AddTrajectory();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CircleTrajectoryPart)) is CircleTrajectoryPart part)
        {
            part.RemoveTrajectory();
        }
    }
}
