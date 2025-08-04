using UnityEngine;

public class CriticalChanceUpPart : SkillPart, ICriticalChancePart
{
    public void InCreaseCriticalChance(float value)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            float maxValue = data.criticalChanceStat.defaultSkillInfo.minMaxValue.y;
            float clampValue = Mathf.Min(data.criticalChanceStat.currentValue + value, maxValue);
            data.criticalChanceStat.currentValue = clampValue;

            if (data.criticalChanceStat.currentValue > 0)
                AddUseSkillStat();
        }
    }

    public void DeCreaseCriticalChance(float value)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            float minValue = data.criticalChanceStat.defaultSkillInfo.minMaxValue.x;
            float clampValue = Mathf.Max(data.criticalChanceStat.currentValue - value, minValue);
            data.criticalChanceStat.currentValue = clampValue;
            
            if (data.criticalChanceStat.currentValue <= minValue)
                RemoveUseSkillStat();
        }
    }
}
