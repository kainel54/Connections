using UnityEngine;

public class ProjectileSkillDescriptionHelper : BaseSkillDescriptionHelper
{
    public override float ReturnData(SkillStatInfoSO currentStatInfo, SkillFieldDataSO currentFieldData)
    {
        return (currentFieldData.skillStatElements[currentStatInfo] as DefaultSkillStatElement).currentValue;
    }
}
