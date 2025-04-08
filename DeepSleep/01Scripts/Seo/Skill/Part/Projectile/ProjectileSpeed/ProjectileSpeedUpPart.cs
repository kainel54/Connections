using UnityEngine;

public class ProjectileSpeedUpPart : SkillPart, IProjectileSpeedUpPart
{
    public void IncreaseMoveSpeed(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projMoveSpeed += count;
        }
    }

    public void DecreaseMoveSpeed(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projMoveSpeed = Mathf.Max(1, data.projMoveSpeed - count);
        }
    }
}
