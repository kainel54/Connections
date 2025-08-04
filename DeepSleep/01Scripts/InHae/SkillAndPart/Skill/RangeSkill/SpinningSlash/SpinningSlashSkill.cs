using System.Collections;
using UnityEngine;

public class SpinningSlashSkill : RangeSkill
{
    [SerializeField] private float _knifePosOffset;
    [SerializeField] private SkillObj _slashSkillObj;
    
    private ProjectileSkillDataSO _projectileDataSO;
    private Transform _playerShootTrm;
    
    public override void UseSkill(Transform shootTrm)
    {
        _playerShootTrm = _playerSkillPointManager.GetTransform(PlayerSkillPointEnum.Player);
        
        _projectileDataSO = GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        base.UseSkill(shootTrm);
    }

    protected override IEnumerator RangeAttack(Transform hipShootTrm)
    {
        for (int i = 1; i <= _genericDataSO!.attackCountStat.currentValue; i++)
        {
            PlaySound();
            Vector3 firstPos = _playerShootTrm.position;
            for (int j = 1; j <= _projectileDataSO!.projectileCountStat.currentValue; j++)
            {
                SkillProjectileObj knifeObj = PoolManager.Instance.Pop(_popSkillObj.PoolEnum) as SkillProjectileObj;
                
                float startAngle = 360 / _projectileDataSO!.projectileCountStat.currentValue * j;
                Quaternion rotation = Quaternion.Euler(0, -startAngle, 0);
                knifeObj.transform.rotation = rotation;

                Vector3 offset = knifeObj.transform.forward * _knifePosOffset;
                knifeObj.transform.position = firstPos + offset;
                
                knifeObj.Initialize(this, hipShootTrm);
                yield return new WaitForSeconds(0.0075f);
            }
            yield return new WaitForSeconds(_genericDataSO.reShootTimeStat.currentValue);
        }   
    }

    public void SlashAttack()
    {
        _forwardVector = _playerShootTrm.forward;
        
        for (int i = 1; i <= _genericDataSO!.attackCountStat.currentValue; i++)
        {
            for (int j = 1; j <= _rangeDataSO!.rangeObjCountStat.currentValue; j++)
            {
                float startAngle = 360 / _rangeDataSO!.rangeObjCountStat.currentValue * j;

                SkillRangeObj rangeObj = PoolManager.Instance.Pop(_slashSkillObj.PoolEnum) as SkillRangeObj;
                
                Quaternion rotation = Quaternion.Euler(0, startAngle, 0);
                Vector3 playerAngleSet = rotation * _forwardVector;
                rangeObj.transform.position = _playerShootTrm.position; //+ playerAngleSet; //<<  It can be another part  
                rangeObj.transform.forward = playerAngleSet.normalized;
                rangeObj.RangeInit(i);
                rangeObj.Initialize(this, _playerShootTrm);
            }
        }
    }
}
