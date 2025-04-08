using System.Collections.Generic;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.StatSystem;

public class Arrow : SkillProjectileObj
{
    [SerializeField] private float _damageInterval = 1;
    private float _damage;
    private Dictionary<Collider, float> _lastDamageTime = new Dictionary<Collider, float>();

    private void Start()
    {
        _damage = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).damage;
        _ispenetration = (skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO).ispenetration;

        OnSkillDestroyEvent += DestroyAction;
    }

    private void DestroyAction()
    {
        DestroyObject(this.gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (_lastDamageTime.TryGetValue(other, out float lastTime))
            {
                if (Time.time - lastTime >= _damageInterval)
                {
                    ApplyDamage(other);
                }
            }
            else
            {
                ApplyDamage(other);
            }
        }
    }

    private void ApplyDamage(Collider collider)
    {
        // todo: apply damage
        if (collider.gameObject.TryGetComponent(out IDamageable damageable))
        {
            Entity entity = skill.player as Entity;
            StatCompo statCompo = entity.GetCompo<StatCompo>();
            damageable.ApplyDamage(statCompo, _damage);
        }

        if (_ispenetration == false)
        {
            CallDestroyEvent();
        }
        // todo: apply other effects like slow, stun, etc.
        _lastDamageTime[collider] = Time.time; // ��ųʸ��� �ݶ��̴��� ������ �ð��� ����
                                               // todo: push this object (object pooling)
    }

    private void OnDestroy()
    {
        OnSkillDestroyEvent -= DestroyAction;
    }
}
