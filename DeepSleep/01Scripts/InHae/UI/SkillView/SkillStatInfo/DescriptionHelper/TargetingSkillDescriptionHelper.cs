using System;
using UnityEngine;

public class TargetingSkillDescriptionHelper : BaseSkillDescriptionHelper
{
    public override float ReturnData(SkillStatInfoSO currentStatInfo, SkillFieldDataSO currentFieldData)
    {
        return (currentFieldData.skillStatElements[currentStatInfo] as DefaultSkillStatElement).currentValue;
    }
}
