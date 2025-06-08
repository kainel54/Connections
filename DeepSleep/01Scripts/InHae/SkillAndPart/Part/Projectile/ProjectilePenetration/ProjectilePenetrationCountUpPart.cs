using UnityEngine;

public class ProjectilePenetrationCountUpPart : SkillPart, IProjectilePenetrationCount
{
    public void SetPenetrationCountDown()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projectilePenetrationCountStat.currentValue--;
        }
    }

    public void SetPenetrationCountUp()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projectilePenetrationCountStat.currentValue++;
        }
    }
}
