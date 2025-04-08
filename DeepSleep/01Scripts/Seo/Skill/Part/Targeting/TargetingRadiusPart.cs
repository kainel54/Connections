using UnityEngine;

public class TargetingRadiusPart : SkillPart, ITargetingRadiusPart
{
    public void DeCreaseTargetingRadius(int radius)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Targeting) is TargetingSkillDataSO data)
        {
            data.canUseSkillRange -= radius;
        }
    }

    public void InCreaseTargetingRadius(int radius)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Targeting) is TargetingSkillDataSO data)
        {
            data.canUseSkillRange += radius;
        }
    }
}
