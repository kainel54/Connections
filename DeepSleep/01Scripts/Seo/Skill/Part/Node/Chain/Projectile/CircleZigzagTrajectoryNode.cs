using UnityEngine;

public class CircleZigzagTrajectoryNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CircleZigzagTrajectoryPart)) is CircleZigzagTrajectoryPart part)
        {
            part.AddTrajectory();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(CircleZigzagTrajectoryPart)) is CircleZigzagTrajectoryPart part)
        {
            part.RemoveTrajectory();
        }
    }
}
