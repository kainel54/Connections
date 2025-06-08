using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileSkill : Skill
{
    [FormerlySerializedAs("projectile")]
    [SerializeField] private SkillProjectileObj skillProjectileObj;

    public override void UseSkill(Transform trm)
    {
        base.UseSkill(trm);
        if (GetShootCount() <= 0)
            SetCoolTime();
        StartCoroutine(ShootingCountTime(trm));
    }
    private IEnumerator ShootingCountTime(Transform shootPos)
    {
        GenericSkillDataSO genericDataSO = GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;
        ProjectileSkillDataSO proDataSO = GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        
        float angleChangeValue = 20f;   // 발사체간의 범위

        Vector3 firePos = shootPos.position + shootPos.forward;
        firePos.y = shootPos.position.y;
        
        for (int i = 0; i < genericDataSO!.attackCountStat.currentValue; i++)
        {
            PlaySound();
            float startAngle = -angleChangeValue * (proDataSO!.projectileCountStat.currentValue - 1) / 2f;

            for (int j = 0; j < proDataSO!.projectileCountStat.currentValue; j++)
            {
                SkillProjectileObj projectile = Instantiate(skillProjectileObj, 
                    firePos + new Vector3(0, 1.1f, 0), Quaternion.identity);
                float angle = startAngle + j * angleChangeValue;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);

                Vector3 playerAngleSet = rotation * shootPos.forward;
                transform.position += playerAngleSet;
                
                projectile.transform.forward = playerAngleSet;
                projectile.Initialize(this, shootPos);

            }
            yield return new WaitForSeconds(genericDataSO.reShootTimeStat.currentValue);
        }

    }
}
