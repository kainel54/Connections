using UnityEngine;

public class RangeSizeUpPart : SkillPart, IRangeSizeUpPart
{
    
    public void InCreaseRangeSize(int size)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO data)
        {
            data.rangeSize += size;
            data.width += size;
            data.height += size;
        }
    }

    public void DeCreaseRangeSize(int size)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO data)
        {
            data.rangeSize -= size;
            data.width -= size;
            data.height -= size;
        }
    }
}
