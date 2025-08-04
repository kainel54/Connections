using System.Collections;
using UnityEngine;

public class RangeSkill : Skill
{
    protected RangeSkillDataSO _rangeDataSO;
    protected GenericSkillDataSO _genericDataSO;
    
    protected Vector3 _forwardVector;
    
    public override void UseSkill(Transform shootTrm)
    {
        base.UseSkill(shootTrm);
        
        _rangeDataSO = GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO;
        _genericDataSO = GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;

        _forwardVector = shootTrm.forward;
        
        StartCoroutine(RangeAttack(shootTrm));
    }

    protected virtual IEnumerator RangeAttack(Transform hipShootTrm)
    {
        for (int i = 1; i <= _genericDataSO!.attackCountStat.currentValue; i++)
        {
            PlaySound();
            for (int j = 1; j <= _rangeDataSO!.rangeObjCountStat.currentValue; j++)
            {
                float startAngle = 360 / _rangeDataSO!.rangeObjCountStat.currentValue * j;

                SkillRangeObj rangeObj = PoolManager.Instance.Pop(_popSkillObj.PoolEnum) as SkillRangeObj;
                
                Quaternion rotation = Quaternion.Euler(0, startAngle, 0);
                Vector3 playerAngleSet = rotation * _forwardVector;
                rangeObj.transform.position = hipShootTrm.position; //+ playerAngleSet; //<<  It can be another part  
                rangeObj.transform.forward = playerAngleSet.normalized;
                rangeObj.RangeInit(i);
                rangeObj.Initialize(this, hipShootTrm);
            }
            yield return new WaitForSeconds(_genericDataSO.reShootTimeStat.currentValue);
        }
    }
}
