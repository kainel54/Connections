using UnityEngine;

public class CoolTimePart : SkillPart, ICoolTimePart
{
    public void DeCreaseCoolTime(float time, ICoolTimePart.ModifyType modifyType)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            switch (modifyType)
            {
                case ICoolTimePart.ModifyType.Add:
                    data.coolTimeStat.currentValue = Mathf.Max(1, data.coolTimeStat.currentValue - time);
                    break;
                case ICoolTimePart.ModifyType.Percent:
                    data.coolTimeStat.currentValue = Mathf.Max(1, data.coolTimeStat.currentValue * (1 - time / 100f));
                    break;
            }
        }
    }

    public void InCreaseCoolTime(float time, ICoolTimePart.ModifyType modifyType)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            switch (modifyType)
            {
                case ICoolTimePart.ModifyType.Add:
                    data.coolTimeStat.currentValue += time;
                    break;
                case ICoolTimePart.ModifyType.Percent:
                    data.coolTimeStat.currentValue += data.coolTimeStat.currentValue * (time / 100f);
                    break;
            }
        }
    }

    public void SetCoolTime(float time)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.coolTimeStat.currentValue = time;
        }
    }

    public void SetCoolTimeBetweenTwo(int time1, int time2)
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            data.coolTimeStat.currentValue = Random.Range(0, 2) == 0 ? time1 : time2;
        }
    }
}
