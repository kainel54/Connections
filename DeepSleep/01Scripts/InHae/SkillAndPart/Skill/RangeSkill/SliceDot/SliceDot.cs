using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Entities;
using YH.StatSystem;

public class SliceDot : SkillRangeObj
{
    private float _radius;
    private LayerMask _enemyMask;
    private RaycastHit[] enemies = new RaycastHit[25];

    public override void Initialize(Skill _skill, Transform shootTrm)
    {
        base.Initialize(_skill, shootTrm);
        
        _radius = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO)
            .rangeAttackSizeStat.currentSphereValue / 2;
        _enemyMask = skill.whatIsEnemy;
        
        Vector3 playerAngleSet = transform.forward;
        transform.position += playerAngleSet;
        
        //transform.localScale = new Vector3(_radius, _radius, _radius);
        OnSkillDestroyEvent += DestroyAction;

        ApplyDamage();
    }

    private void DestroyAction()
    {
        DestroyObject(this.gameObject, 1);
    }

    private void ApplyDamage()
    {
        int count = Physics.SphereCastNonAlloc(transform.position, _radius, transform.forward, enemies, 0, _enemyMask);

        for (int i = 0; i < count; i++)
        {
            if (enemies[i].collider.gameObject.TryGetComponent(out IDamageable damageable))
            {

                Vector3 hitObjtoDir = (enemies[i].transform.position - skill.player.transform.position).normalized;
                float dotValue = Vector3.Dot(hitObjtoDir, skill.player.transform.forward);  // �������� �յ� �Ǻ��ؼ� �� ���� �տ� ������ ������ ����.
                                                                                            // ��������� �ݿ����θ� �������� �ִ°���.
                if (dotValue > 0)
                {
                    damageable.ApplyDamage(GetHitData());
                    CameraManager.Instance.ShakeCamera(1, 1, .3f);
                }
            }
        }

        CallDestroyEvent();
    }

    private void OnDestroy()
    {
        OnSkillDestroyEvent -= DestroyAction;
    }
}
