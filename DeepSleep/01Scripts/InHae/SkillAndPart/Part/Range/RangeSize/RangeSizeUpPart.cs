using UnityEngine;

public class RangeSizeUpPart : SkillPart, IRangeSizePart
{
    public void InCreaseRangeSize(int size)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO data)
        {
            data.rangeAttackSizeStat.currentSphereValue += size;
            data.rangeAttackSizeStat.currentWidthValue += size;
            data.rangeAttackSizeStat.currentHeightValue += size;
        }
    }

    public void DeCreaseRangeSize(int size)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Range) is RangeSkillDataSO data)
        {
            data.rangeAttackSizeStat.currentSphereValue -= size;
            data.rangeAttackSizeStat.currentWidthValue -= size;
            data.rangeAttackSizeStat.currentHeightValue -= size;
        }
    }
}
