using UnityEngine;

public class AttackCountUpPart : SkillPart, IAttackCountUpPart
{
    public void InCreaseAttackCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.attackCount += count;
        }
    }
    public void DeCreaseAttackCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.attackCount = Mathf.Max(1, data.attackCount - count);
        }
    }
}
