using System.Collections.Generic;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.StatSystem;

public class FireCircle : SkillTargetingObj
{
    [SerializeField] private ParticleSystem[] _particles;
    private float _radius;
    private float _duration;
    private float _damageDelay;

    private Dictionary<Collider, float> _lastDamageTime = new Dictionary<Collider, float>();

    private float _addTime = 0;

    public override void Initialize(Skill _skill, Transform shootTrm)
    {
        base.Initialize(_skill, shootTrm);
                
        _radius = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO)
            .rangeAttackSizeStat.currentSphereValue / 2;
        _damageDelay = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).skillDamageDelay;
        _duration = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).skillActiveDurationStat.currentValue;
        
        transform.localScale = new Vector3(_radius, _radius, _radius);
        SetDuration(_duration);

        OnSkillDestroyEvent += DestroyAction;
    }

    private void DestroyAction()
    {

    }

    private void Update()
    {
        _addTime += Time.deltaTime;
        if (_addTime >= _duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            if (_lastDamageTime.TryGetValue(other, out float lastTime))
            {
                if (Time.time - lastTime >= _damageDelay)
                {
                    ApplyDamage(other);
                }
            }
            else
            {
                _lastDamageTime.Add(other, Time.time);
                ApplyDamage(other);
            }
        }
    }

    private void ApplyDamage(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.ApplyDamage(GetHitData());
        }

        _lastDamageTime[other] = Time.time;
    }

    private void SetDuration(float duration)
    {
        foreach (ParticleSystem obj in _particles)
        {
            var temObj = obj.main;
            temObj.startLifetime = duration;
        }
    }

    private void OnParticleSystemStopped()
    {
        CallDestroyEvent();
    }

    private void OnDestroy()
    {
        OnSkillDestroyEvent -= DestroyAction;
    }
}
