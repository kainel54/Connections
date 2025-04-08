using UnityEngine;

public class CircleTrajectoryPart : SkillPart, ITrajectoryPart
{
    public void AddTrajectory()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        { 
            skillData.currentTrajectoryList.Add(TrajectoryType.Circle);
        }
    }

    public void RemoveTrajectory()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        {
            skillData.currentTrajectoryList.Remove(TrajectoryType.Circle);
        }
    }
}
