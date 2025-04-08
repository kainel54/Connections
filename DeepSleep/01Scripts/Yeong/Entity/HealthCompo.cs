using DG.Tweening;
using YH.StatSystem;
using ObjectPooling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using YH.Combat;
using YH.Entities;
using YH.Players;
using Random = UnityEngine.Random;
using YH.Core;

public class HealthCompo : MonoBehaviour, IEntityComponent, IAfterInitable, IDamageable
{
    public float Health { get; private set; }

    public float LastAttackTime { get; set; }
    [SerializeField] private StatElementSO _healthSO;

    private Entity _owner;
    private StatElement _maxHealth;
    private bool _isInvincible;
    private bool _isDie;
    private StatCompo _statCompo;

    public int MaxHealth => Mathf.CeilToInt(_maxHealth.Value);
    public UnityEvent<float, float, bool> OnHealthChangedEvent;
    public UnityEvent OnDieEvent;

    public void Initialize(Entity entity)
    {
        _owner = entity;
        _statCompo = _owner.GetCompo<StatCompo>();
    }

    public void SetInvincible(bool isInvinvible)
    {
        _isInvincible = isInvinvible;
    }

    public void AfterInit()
    {
        _maxHealth = _statCompo.GetElement(_healthSO);
        _isInvincible = _maxHealth == null;
        Health = MaxHealth;

        if (_maxHealth != null) _maxHealth.OnValueChanged += HandleMaxHealthChangedEvent;
    }

    private void HandleMaxHealthChangedEvent(float prev, float current)
    {
        float prevHealth = Health;
        if (Health > current) Health = Mathf.CeilToInt(current);
        OnHealthChangedEvent?.Invoke(prevHealth, Health, false);
    }

    public void ApplyDamage(StatCompo statCompo, float damage, bool isChangeVisible = true, bool isTextVisible = true)
    {
        if (_isDie) return;
        if (_isInvincible) return;

        bool isCritical = false;
        float random = Random.Range(0f, 100f);
        Debug.Log(statCompo.transform.root.name);
        //damage = 100 / (100 + statCompo.GetElement("Defense").Value) * damage;
        //damage = damage * Mathf.Log(damage / statCompo.GetElement("Defense").Value * 10);
        damage = damage * Mathf.Log10(damage/_statCompo.GetElement("Defense").Value * 10);
        if (random < statCompo.GetElement("Critical").Value)
        {
            isCritical = true;
            damage *= (statCompo.GetElement("CriticalDamage").Value / 100);
        }

        if(_owner as Player)
        {
            CameraManager.Instance.ShakeCamera(2, 2, 0.15f);
        }

        float prev = Health;
        Health -= damage;
        if (Health < 0)
            Health = 0;
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);



        if (isTextVisible)
        {
            DamageText damageText = PoolManager.Instance.Pop(PoolingType.DamageText) as DamageText;
            damageText.Setting((int)damage, isCritical, transform.position);
        }

        if (Health == 0) Die();
    }

    public void ApplyRecovery(int recovery, bool isChangeVisible = true)
    {
        if (_isDie) return;

        float prev = Health;
        Health += recovery;
        if (Health > MaxHealth)
            Health = MaxHealth;
        //OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);
    }

    public void Resurrection()
    {
        _isDie = false;
        ApplyRecovery(MaxHealth, false);
    }

    public void Die()
    {
        if(_isDie)
            return;
        
        _isDie = true;
        
        OnDieEvent?.Invoke();
        if (_maxHealth != null) _maxHealth.OnValueChanged -= HandleMaxHealthChangedEvent;
    }

}
