using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileSkill : Skill
{
    [FormerlySerializedAs("projectile")][SerializeField] private SkillProjectileObj skillProjectileObj;

    public override void UseSkill(Transform trm)
    {
        base.UseSkill(trm);
        if (shootCount <= 0)
            SetCoolTime();
        StartCoroutine(ShootingCountTime(trm));
    }
    private IEnumerator ShootingCountTime(Transform shootPos)
    {
        GenericSkillDataSO genericDataSO = GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;
        ProjectileSkillDataSO proDataSO = GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;

        float angleChangeValue = 20f;   // 발사체간의 범위


        for (int j = 0; j < genericDataSO!.attackCount; j++)
        {
            float startAngle = -angleChangeValue * (proDataSO!.skillObjCreateCount - 1) / 2f;

            for (int i = 0; i < proDataSO!.skillObjCreateCount; i++)
            {

                SkillProjectileObj arrow = Instantiate(skillProjectileObj, shootPos.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
                float angle = startAngle + i * angleChangeValue;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);

                Vector3 playerAngleSet = rotation * shootPos.forward;
                arrow.transform.forward = playerAngleSet;
                arrow.Initialize(this);

            }
            yield return new WaitForSeconds(genericDataSO.reShootTime);
        }

    }
}
