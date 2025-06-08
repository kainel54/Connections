using UnityEngine;

public class AttackCountUpPart : SkillPart, IAttackCountPart
{
    public void InCreaseAttackCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.attackCountStat.currentValue += count;
            AddUseSkillStat();
        }
    }
    public void DeCreaseAttackCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.attackCountStat.currentValue = Mathf.Max(data.attackCountStat.defaultSkillInfo.minMaxValue.x, 
                data.attackCountStat.currentValue - count);
            
            if(data.attackCountStat.currentValue <= 1)
                RemoveUseSkillStat();
        }
    }
}
