using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Entities;
using YH.StatSystem;

public class SliceDot : SkillRangeObj
{
    private float _radius;
    private float _damage;
    private LayerMask _enemyMask;
    private RaycastHit[] enemies = new RaycastHit[25];


    private void Start()
    {
        _radius = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO).rangeSize / 2;
        _damage = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).damage;
        _enemyMask = skill.whatIsEnemy;
        transform.position = transform.position + transform.forward * (shootCount - 1) * _radius;
        // transform.localScale = new Vector3(_radius, _radius, _radius);
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

        Entity entity = skill.player as Entity;
        StatCompo statCompo = entity.GetCompo<StatCompo>();

        for (int i = 0; i < count; i++)
        {
            if (enemies[i].collider.gameObject.TryGetComponent(out IDamageable damageable))
            {

                Vector3 hitObjtoDir = (enemies[i].transform.position - skill.player.transform.position).normalized;
                float dotValue = Vector3.Dot(hitObjtoDir, skill.player.transform.forward);  // �������� �յ� �Ǻ��ؼ� �� ���� �տ� ������ ������ ����.
                                                                                            // ��������� �ݿ����θ� �������� �ִ°���.
                if (dotValue > 0)
                {
                    damageable.ApplyDamage(statCompo, _damage);
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
