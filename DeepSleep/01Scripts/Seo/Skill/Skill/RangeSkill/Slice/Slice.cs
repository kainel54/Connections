using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Entities;
using YH.StatSystem;

public class Slice : SkillRangeObj
{
    private float _radius;
    private float _damage;
    private LayerMask _enemyMask;


    private void Start()
    {
        _radius = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO).rangeSize / 2;
        _damage = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).damage;
        _enemyMask = skill.whatIsEnemy;
        transform.position = transform.position + transform.forward * (shootCount - 1) * _radius;
        //transform.localScale = new Vector3(_radius, _radius, _radius) * 2;
        OnSkillDestroyEvent += DestroyAction;

        ApplyDamage();
    }

    private void DestroyAction()
    {
        DestroyObject(this.gameObject, 1);
    }

    private void ApplyDamage()
    {
        RaycastHit[] enemies;
        enemies = Physics.SphereCastAll(transform.position, _radius, transform.forward, 0, _enemyMask);
        Entity entity = skill.player as Entity;
        StatCompo statCompo = entity.GetCompo<StatCompo>();

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].collider.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(statCompo, _damage);
                CameraManager.Instance.ShakeCamera(1, 1, .3f);
            }
        }

        CallDestroyEvent();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }

    private void OnDestroy()
    {
        OnSkillDestroyEvent -= DestroyAction;
    }
}
