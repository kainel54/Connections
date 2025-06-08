using UnityEngine;

public class DamageUpPart : SkillPart, IDamagePart
{
    public void InCreaseDamage(int value)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.attackDamageStat.currentValue += value;
        }
    }

    public void DeCreaseDamage(int value)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.attackDamageStat.currentValue = Mathf.Max(1, data.attackDamageStat.currentValue - value);
        }
    }
}
