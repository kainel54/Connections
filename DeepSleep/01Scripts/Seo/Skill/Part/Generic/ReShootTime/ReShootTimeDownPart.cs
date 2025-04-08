using UnityEngine;

public class ReShootTimeDownPart : SkillPart, IReShootTimeDownPart
{
    public void DecreaseReShootTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.reShootTime = Mathf.Max(0.2f, data.reShootTime - time);
        }
    }
    
    public void IncreaseReShootTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.reShootTime += time;
        }
    }
}
