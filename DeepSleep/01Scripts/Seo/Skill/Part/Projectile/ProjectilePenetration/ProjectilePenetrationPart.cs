using UnityEngine;

public class ProjectilePenetrationPart : SkillPart, IProjectilePenetrationSetting
{
    public void SetPenetrationFalse()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.ispenetration = false;
        }
    }

    public void SetPenetrationTrue()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.ispenetration = true;
        }
    }
}
