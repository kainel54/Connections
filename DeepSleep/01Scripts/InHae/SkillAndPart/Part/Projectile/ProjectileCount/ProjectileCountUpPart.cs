using UnityEngine;

public class ProjectileCountUpPart : SkillPart, IProjectileCountPart
{
    public void IncreaseProjectileCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        {
            skillData.projectileCountStat.currentValue += count;
        }
    }

    public void DecreaseProjectileCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        {
            skillData.projectileCountStat.currentValue 
                = Mathf.Max(skillData.projectileCountStat.defaultSkillInfo.minMaxValue.x, 
                    skillData.projectileCountStat.currentValue - count);
        }
    }
}
