using YH.StatSystem;
using ObjectPooling;
using System;
using UnityEngine;
using UnityEngine.Events;
using YH.Combat;
using YH.Entities;
using YH.Players;
using Random = UnityEngine.Random;
using YH.Core;

public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInitable, IDamageable
{
    public float Health { get; private set; }

    public float LastAttackTime { get; set; }
    
    [SerializeField] private StatElementSO _healthSO;

    private Entity _owner;
    private StatElement _maxHealth;
    private bool _isInvincible;
    private bool _isDie;
    private EntityStat _statCompo;

    public int MaxHealth => Mathf.CeilToInt(_maxHealth.Value);
    
    [Header("Damage or Heal Event Ex)HealthBar")]
    public UnityEvent<float, float, bool> OnHealthChangedEvent;
    [Header("Only Hit Event Ex)HitSound, BloodScreen")]
    public UnityEvent OnHitEvent;
    public UnityEvent OnDieEvent;
    
    public event Action<Entity> OnDeadEvent;

    public void Initialize(Entity entity)
    {
        _owner = entity;
        _statCompo = _owner.GetCompo<EntityStat>();
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

        if (_maxHealth != null)
            _maxHealth.OnValueChanged += HandleMaxHealthChangedEvent;
    }

    private void HandleMaxHealthChangedEvent(float prev, float current)
    {
        float prevHealth = Health;
        if (Health > current)
            Health = Mathf.CeilToInt(current);
        
        OnHealthChangedEvent?.Invoke(prevHealth, Health, false);
    }

    public void ApplyDamage(HitData hitData, bool isChangeVisible = true, bool isTextVisible = true, float damageDecrease = 1)
    {
        if (_isDie) return;
        if (_isInvincible) return;

        bool isCritical = false;
        
        float damage = hitData.damage;
        float random = Random.Range(0f, 100f);
        
        //damage = 100 / (100 + statCompo.GetElement("Defense").Value) * damage;
        //damage = damage * Mathf.Log(damage / statCompo.GetElement("Defense").Value * 10);
        
        damage = damage * Mathf.Log10(damage / _statCompo.GetElement("Defense").Value * 10) * damageDecrease;
        if (random < hitData.ciriticalChance)
        {
            isCritical = true;
            damage *= (hitData.ciriticalDamage / 100);
        }

        if(_owner as Player)
        {
            CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
        }

        float prev = Health;
        Health -= damage;
        if (Health < 0)
            Health = 0;
        
        OnHitEvent?.Invoke();
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);

        if (isTextVisible)
        {
            DamageText damageText = PoolManager.Instance.Pop(UIPoolingType.DamageText) as DamageText;
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
        
        OnHealthChangedEvent?.Invoke(prev, Health, isChangeVisible);
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
        OnDeadEvent?.Invoke(_owner);
        if (_maxHealth != null) _maxHealth.OnValueChanged -= HandleMaxHealthChangedEvent;
    }

    public bool GetDie()
    {
        return _isDie;
    }

    public void Dispose()
    {

    }
}
