using UnityEngine;

public class TargetingRadiusUpPart : SkillPart, ITargetingRadiusPart
{
    public void DeCreaseTargetingRadius(int radius)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Targeting) is TargetingSkillDataSO data)
        {
            data.canUseSkillRangeStat.currentValue -= radius;
        }
    }

    public void InCreaseTargetingRadius(int radius)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Targeting) is TargetingSkillDataSO data)
        {
            data.canUseSkillRangeStat.currentValue += radius;
        }
    }
}
