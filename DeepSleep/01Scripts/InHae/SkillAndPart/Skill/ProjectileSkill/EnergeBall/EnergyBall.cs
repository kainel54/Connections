using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using YH.Combat;

public class EnergyBall : SkillProjectileObj
{
    [SerializeField] private float _damageInterval = 1;
    [SerializeField] private List<Transform> _particles;
    
    private Dictionary<Collider, float> _lastDamageTime = new Dictionary<Collider, float>();

    public override void Initialize(Skill _skill, Transform shootTrm)
    {
        base.Initialize(_skill, shootTrm);
        _canBeHit = (skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO).canBeHit;
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
        if (collider.gameObject.CompareTag("Enemy")|| collider.gameObject.CompareTag("Tower"))
        {
            SoundPlay(_hitSound);
            
            if (collider.gameObject.TryGetComponent(out IDamageable damageable))
            {
                ImpactEffectPlay();
                damageable.ApplyDamage(GetHitData());
            }

            if (_penetrationCount <= _currentPenetrationCount)
                CallDestroyEvent();
            else
                _currentPenetrationCount++;
            
            // todo: apply other effects like slow, stun, etc.
            _lastDamageTime[collider] = Time.time; // 딕셔너리에 콜라이더와 마지막 시간을 저장
        }
        if (collider.gameObject.CompareTag("Bullet"))
        {
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
}