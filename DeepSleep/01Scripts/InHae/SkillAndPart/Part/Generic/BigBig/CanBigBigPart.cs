using UnityEngine;

public class CanBigBigPart : SkillPart, ICanBigBigPart
{
    public void BigbigEquip()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.canBeHit = true;
        }
    }

    public void BigBigUnEquip()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO data)
        {
            data.canBeHit = false;
        }
    }
}
