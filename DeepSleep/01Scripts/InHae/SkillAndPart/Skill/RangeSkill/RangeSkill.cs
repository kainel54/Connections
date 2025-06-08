using System.Collections;
using UnityEngine;

public class RangeSkill : Skill
{
    [SerializeField] private SkillRangeObj skillRangeObj;

    private RangeSkillDataSO _rangeDataSO;
    private GenericSkillDataSO _genericDataSO;
    public override void UseSkill(Transform shootTrm)
    {
        base.UseSkill(shootTrm);
        if (GetShootCount() <= 0)
            SetCoolTime();
        
        _rangeDataSO = GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO;
        _genericDataSO = GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;

        StartCoroutine(RangeAttack(shootTrm));
    }

    private IEnumerator RangeAttack(Transform shootTrm)
    {
        for (int i = 1; i <= _genericDataSO!.attackCountStat.currentValue; i++)
        {
            PlaySound();
            for (int j = 0; j < _rangeDataSO!.rangeObjCountStat.currentValue; j++)
            {
                float startAngle = 360 / _rangeDataSO!.rangeObjCountStat.currentValue * (j + 1);

                SkillRangeObj range = Instantiate(skillRangeObj, shootTrm.position + new Vector3(0, 1.1f, 0), Quaternion.identity);

                Quaternion rotation = Quaternion.Euler(0, startAngle, 0);
                Vector3 playerAngleSet = rotation * shootTrm.forward;
                range.transform.position = shootTrm.position; //+ playerAngleSet; //<<  It can be another part  
                range.transform.forward = playerAngleSet.normalized;
                range.RangeInit(i);
                range.Initialize(this, shootTrm);
            }
            yield return new WaitForSeconds(_genericDataSO.reShootTimeStat.currentValue);
        }
    }
}
