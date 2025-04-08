using UnityEngine;

public class ZigzagTrajectoryNode : PartNode
{
    public override void EquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ZigzagTrajectoryPart)) is ZigzagTrajectoryPart part)
        {
            part.AddTrajectory();
        }
    }

    public override void UnEquipPart(Skill skill)
    {
        if (skill.GetSkillPart(typeof(ZigzagTrajectoryPart)) is ZigzagTrajectoryPart part)
        {
            part.RemoveTrajectory();
        }
    }
}
