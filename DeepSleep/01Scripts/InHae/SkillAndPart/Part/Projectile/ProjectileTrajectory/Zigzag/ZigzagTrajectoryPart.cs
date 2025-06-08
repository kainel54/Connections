using UnityEngine;

public class ZigzagTrajectoryPart : SkillPart, ITrajectoryPart
{
    public void AddTrajectory()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        {
            skillData.projectileTrajectoryStat.currentTrajectory.Add(TrajectoryType.ZigZag);
        }
    }

    public void RemoveTrajectory()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        {
            skillData.projectileTrajectoryStat.currentTrajectory.Remove(TrajectoryType.ZigZag);
        }
    }
}
