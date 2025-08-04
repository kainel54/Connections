using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YH.Combat;

public class SkillDamageCasterParent : MonoBehaviour
{
    [Header("0일 경우 상관없이 전부 다 타격")]
    [SerializeField] private int _maxHitCount;
    
    [SerializeField] private float _damageDelay;
    
    private List<SkillDamageCaster> _casters;
    private Dictionary<Collider, float> _lastDamageTime = new ();
    private HashSet<Collider> _hitEnemies = new();
    
    private SkillObj _ownerSkillObj;
    private LayerMask _enemyLayerMask;

    public event Action<Collider> HitAction;
    public bool IsOnceCheck {get; private set;}
    
    private void Awake()
    {
        _casters = GetComponentsInChildren<SkillDamageCaster>().ToList();
        CasterEnable(false);
    }
    
    public void Init(SkillObj skillObj, bool isOnceCheck)
    {
        HitAction = null;
        _lastDamageTime.Clear();
        _hitEnemies.Clear();
        
        _ownerSkillObj = skillObj;
        _enemyLayerMask = _ownerSkillObj.skill.whatIsEnemy;
        IsOnceCheck = isOnceCheck;
        
        foreach (var caster in _casters)
        {
            caster.Init(skillObj, this);
            caster.gameObject.SetActive(true);
        }
    }

    public void StayCastDamageCheck(Collider other)
    {
        if (_damageDelay <= 0)
        {
            if (_lastDamageTime.ContainsKey(other))
                return;

            ApplyDamage(other);
            _lastDamageTime.Add(other, 0f);
        }
        else
        {
            LastDamageTimeCheck(other);
        }
    }

    private void LastDamageTimeCheck(Collider other)
    {
        if (_lastDamageTime.TryGetValue(other, out float lastTime))
        {
            if (Time.time - lastTime >= _damageDelay)
            {
                ApplyDamage(other);
                _lastDamageTime[other] = Time.time;
            }
        }
        else
        {
            ApplyDamage(other);
            _lastDamageTime.Add(other, Time.time);
        }
    }
    
    public void ApplyDamage(Collider other)
    {
        if (((1 << other.gameObject.layer) & _enemyLayerMask) == 0)
            return;
        if(!other.gameObject.TryGetComponent(out IDamageable damageable)) 
            return;
        if(_maxHitCount != 0 && _hitEnemies.Count >= _maxHitCount && !_hitEnemies.Contains(other))
            return;

        Debug.Log(_hitEnemies.Count);
        _hitEnemies.Add(other);
        if (IsOnceCheck)
        {
            damageable.ApplyDamage(_ownerSkillObj.GetHitData());
            HitAction?.Invoke(other);
            return;
        }

        damageable.ApplyDamage(_ownerSkillObj.GetHitData());
        HitAction?.Invoke(other);
    }
    
    public void CasterEnable(bool isEnable)
    {
        foreach (var caster in _casters)
            caster.gameObject.SetActive(isEnable);
    }
}