using UnityEngine;

public class ProjectileCountUpPart : SkillPart, IProjectileCountUpPart
{
    public void IncreaseProjectileCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        {
            skillData.skillObjCreateCount += count;
        }
    }

    public void DecreaseProjectileCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO skillData)
        {
            skillData.skillObjCreateCount = Mathf.Max(1, skillData.skillObjCreateCount - count);
        }
    }
}
