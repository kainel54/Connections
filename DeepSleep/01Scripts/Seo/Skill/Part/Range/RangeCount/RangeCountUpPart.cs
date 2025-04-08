using UnityEngine;

public class RangeCountUpPart : SkillPart, IRangeCountUpPart
{
    public void IncreaseRangeCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO skillData)
        {
            skillData.skillObjCreateCount += count;
        }
    }
    public void DecreaseRangeCount(int count)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO skillData)
        {
            skillData.skillObjCreateCount = Mathf.Max(1, skillData.skillObjCreateCount - count);
        }
    }
}
