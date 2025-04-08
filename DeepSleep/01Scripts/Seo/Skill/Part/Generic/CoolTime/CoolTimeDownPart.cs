using UnityEngine;

public class CoolTimeDownPart : SkillPart, ICoolTimeDownPart
{
    public void DeCreaseCoolTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.coolTime = Mathf.Max(1, data.coolTime - time);
        }
    }

    public void InCreaseCoolTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.coolTime += time;
        }
    }

    public void SetCoolTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.coolTime = time;
        }
    }

    public void SetCoolTimeBetweenTwo(int time1, int time2)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.coolTime = Random.Range(0, 2) == 0 ? time1 : time2;
        }
    }
}
