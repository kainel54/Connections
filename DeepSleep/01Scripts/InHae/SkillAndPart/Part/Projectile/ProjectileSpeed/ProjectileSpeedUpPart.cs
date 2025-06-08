using UnityEngine;

public class ProjectileSpeedUpPart : SkillPart, IProjectileSpeedPart
{
    public void IncreaseMoveSpeed(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projectileMoveSpeedStat.currentValue += count;
        }
    }

    public void DecreaseMoveSpeed(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projectileMoveSpeedStat.currentValue = 
                Mathf.Max(1, data.projectileMoveSpeedStat.currentValue - count);
        }
    }
}
