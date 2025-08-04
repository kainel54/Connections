using System;
using System.Collections;
using ObjectPooling;
using UnityEngine;
using YH.Combat;
using IPoolable = ObjectPooling.IPoolable;

public class SkillObj : MonoBehaviour, IPoolable
{
    [SerializeField] private PlayerSkillPoolingType _playerSkillPoolingType;
    [HideInInspector] public Skill skill;
    
    public event Action OnSkillEndEvent;

    protected float _damage;
    protected float _criticalChance;
    protected float _criticalDamage = 100f;

    protected bool _isInited;
    private Vector3 _defaultScale;

    protected virtual void Awake()
    {
        _defaultScale = transform.localScale;
    }

    protected virtual void OnDestroy()
    {
        OnSkillEndEvent = null;
    }

    public virtual void Initialize(Skill currentSkill, Transform shootTrm)
    {
        skill = currentSkill;
        
        GenericSkillDataSO genericSkillDataSo = skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;
        
        _damage = genericSkillDataSo.attackDamageStat.currentValue;
        _criticalChance = genericSkillDataSo.criticalChanceStat.currentValue;
        _criticalDamage = genericSkillDataSo.criticalDamageStat.currentValue;

        _isInited = true;
    }
    
    public HitData GetHitData() => new HitData(skill.player, _damage, _criticalChance, _criticalDamage);

    protected void SelfPoolPush(float timer = 0f, bool isParentReset = false)
    {
        if(!gameObject.activeInHierarchy)
            return;
        
        StartCoroutine(SetPushTimerRoutine(timer, isParentReset));
    }

    private IEnumerator SetPushTimerRoutine(float timer, bool isParentReset = false)
    {
        yield return new WaitForSeconds(timer);
        PoolManager.Instance.Push(this, isParentReset);
    }
    
    public GameObject GameObject => gameObject;
    public Enum PoolEnum => _playerSkillPoolingType;
    
    public virtual void OnPop()
    {
        transform.localScale = _defaultScale;
    }

    public virtual void OnPush()
    {
        _isInited = false;
        OnSkillEndEvent?.Invoke();
    }
}
