using UnityEngine;

public class DamageUpPart : SkillPart, IDamageUpPart
{
    public void InCreaseDamage(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.damage += count;
        }
    }

    public void DeCreaseDamage(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.damage = Mathf.Max(1, data.damage - count);

        }
    }
}
