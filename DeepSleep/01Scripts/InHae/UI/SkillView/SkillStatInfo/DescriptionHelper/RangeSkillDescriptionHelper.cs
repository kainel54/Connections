using System;
using UnityEngine;

public class RangeSkillDescriptionHelper : BaseSkillDescriptionHelper
{
    public override float ReturnData(SkillStatInfoSO currentStatInfo, SkillFieldDataSO currentFieldData)
    {
        float value = 0;
        if (currentFieldData.skillStatElements[currentStatInfo] is RangeAttackSizeSkillStatElement rangeSkillStatElement)
        {
            switch (rangeSkillStatElement.attackType)
            {
                case RangeSkillAttackType.Sphere:
                    value = rangeSkillStatElement.currentSphereValue;
                    break;
                case RangeSkillAttackType.Square:
                    value = rangeSkillStatElement.currentHeightValue * rangeSkillStatElement.currentWidthValue;
                    break;
            }
        }

        return value;
    }
}
