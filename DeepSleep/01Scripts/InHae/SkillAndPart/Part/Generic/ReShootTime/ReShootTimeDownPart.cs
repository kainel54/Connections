using UnityEngine;

public class ReShootTimeDownPart : SkillPart, IReShootTimePart
{
    public void DecreaseReShootTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.reShootTimeStat.currentValue = Mathf.Max(0.2f, data.reShootTimeStat.currentValue - time);
        }
    }
    
    public void IncreaseReShootTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.reShootTimeStat.currentValue += time;
        }
    }
}
