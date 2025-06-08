public class ReflectionCountUpPart : SkillPart, IReflectionPart
{
    public void ReflectionEquip()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projectileReflectionCountStat.currentValue += 1;
        }
    }

    public void ReflectionUnEquip()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.projectileReflectionCountStat.currentValue -= 1;
        }
    }
}
