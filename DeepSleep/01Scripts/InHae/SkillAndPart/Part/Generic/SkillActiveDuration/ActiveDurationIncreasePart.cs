using UnityEngine;

public class ActiveDurationIncreasePart : SkillPart, IActiveDurationPart
{
    public void InCreaseActiveDuration(int value)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.skillActiveDurationStat.currentValue += value;
        }
    }

    public void DeCreaseActiveDuration(int value)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.skillActiveDurationStat.currentValue = Mathf.Max(1, data.skillActiveDurationStat.currentValue - value);
        }
    }
}
