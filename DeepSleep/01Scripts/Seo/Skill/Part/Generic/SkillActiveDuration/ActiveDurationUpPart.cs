using UnityEngine;

public class ActiveDurationUpPart : SkillPart, IActiveDurationUpPart
{
    public void InCreaseActiveDuration(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.skillActiveDuration += count;
        }
    }

    public void DeCreaseActiveDuration(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.skillActiveDuration = Mathf.Max(1, data.skillActiveDuration - count);
        }
    }
}
