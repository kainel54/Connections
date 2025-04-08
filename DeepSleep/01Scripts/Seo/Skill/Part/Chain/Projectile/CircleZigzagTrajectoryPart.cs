using UnityEngine;

public class CircleZigzagTrajectoryPart : SkillPart, ITrajectoryPart
{
    public void AddTrajectory()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        { 
            skillData.currentTrajectoryList.Add(TrajectoryType.Circle);
            skillData.currentTrajectoryList.Add(TrajectoryType.ZigZag);
        }
    }

    public void RemoveTrajectory()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        { 
            skillData.currentTrajectoryList.Remove(TrajectoryType.Circle);
            skillData.currentTrajectoryList.Remove(TrajectoryType.ZigZag);
        }
    }
}
