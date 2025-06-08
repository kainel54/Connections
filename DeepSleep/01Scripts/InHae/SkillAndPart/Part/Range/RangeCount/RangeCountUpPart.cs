using UnityEngine;

public class RangeCountUpPart : SkillPart, IRangeCountPart
{
    public void IncreaseRangeCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO skillData)
        {
            skillData.rangeObjCountStat.currentValue += count;
        }
    }
    public void DecreaseRangeCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO skillData)
        {
            skillData.rangeObjCountStat.currentValue = 
                Mathf.Max(1, skillData.rangeObjCountStat.currentValue - count);
        }
    }
}
