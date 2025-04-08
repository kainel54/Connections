using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.StatSystem;

public class EnergyBall : SkillProjectileObj
{
    [SerializeField] private float _damageInterval = 1;
    [SerializeField] private List<Transform> _particles;

    private float _damage;

    private Dictionary<Collider, float> _lastDamageTime = new Dictionary<Collider, float>();

    private void Start()
    {
        _damage = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).damage;
        _ispenetration = (skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO).ispenetration;
        _canBeHit = (skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO).canBeHit;

        OnSkillDestroyEvent += DestroyAction;
    }

    private void DestroyAction()
    {
        DestroyObject(this.gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        ApplyDamage(other);
    }

    private void OnTriggerStay(Collider other)
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

    private void ApplyDamage(Collider collider)
    {
        // todo: apply damage
        if (collider.gameObject.CompareTag("Enemy"))
        {
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
            _lastDamageTime[collider] = Time.time; // 딕셔너리에 콜라이더와 마지막 시간을 저장
        }
        if (collider.gameObject.CompareTag("Bullet"))
        {
            Debug.Log(_canBeHit);
            Debug.Log(_damage);
            if (_canBeHit)
            {
                foreach (Transform particle in _particles)
                {
                    particle.DOScale(particle.localScale * 1.05f, 0.3f);
                }

                _damage += 0.5f; // todo: 중첩 배수 넣기
            }
        }
    }

    private void OnDestroy()
    {
        OnSkillDestroyEvent -= DestroyAction;
    }
}