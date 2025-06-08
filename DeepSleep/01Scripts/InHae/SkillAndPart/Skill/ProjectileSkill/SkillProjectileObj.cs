using System;
using System.Collections;
using IH.EventSystem.SoundEvent;
using ObjectPooling;
using UnityEngine;
using YH.EventSystem;

public class SkillProjectileObj : SkillObj
{
    [SerializeField] private GameEventChannelSO _spawnEventChannelSO;
    [SerializeField] private PoolingItemSO _impactEffectPoolSO;
    [SerializeField] private GameEventChannelSO _soundEventChannelSO;
    [SerializeField] protected SoundSO _hitSound;

    [SerializeField] protected float _lifetime = 17;
    
    private TrajectoryManager _trajectoryManager;
    private BaseTrajectory _trajectory;

    protected bool _canBeHit = false;

    public Action<Vector3> ReflectEvent;
    private Vector3 dir;

    private int _currentReflectionCount;
    
    protected int _penetrationCount;
    protected int _currentPenetrationCount;
    
    private Coroutine _coroutine;

    protected virtual void Awake()
    {
        _trajectoryManager = GetComponentInChildren<TrajectoryManager>();

        OnSkillDestroyEvent += HandleProjectileDestroy;
    }

    protected virtual void OnDestroy()
    {
        OnSkillDestroyEvent -= HandleProjectileDestroy;
    }

    protected virtual void HandleProjectileDestroy()
    {
        ImpactEffectPlay();
        Destroy(gameObject);
    }

    public override void Initialize(Skill _skill, Transform shootTrm)
    {
        base.Initialize(_skill, shootTrm);
        _penetrationCount = 
            (int)(skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO)
            .projectilePenetrationCountStat.currentValue;
        _currentPenetrationCount = 0;
        
        SetTrajectory();

        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(LifeTimeRoutine());
    }

    private IEnumerator LifeTimeRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        CallDestroyEvent();
    }

    public void SetTrajectory()
    {
        if (skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO projectileSkillDataSO)
        {
            _trajectory = _trajectoryManager.GetTrajectory(projectileSkillDataSO.projectileTrajectoryStat.currentTrajectory);
            _trajectory.Init(this);
        }
    }

    protected virtual void FixedUpdate()
    {
        dir = _trajectory.UpdateTrajectory();
        transform.forward = dir;
        transform.position += dir;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") 
            || other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if ((skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO)
                .projectileReflectionCountStat.currentValue > _currentReflectionCount)
            {
                ImpactEffectPlay();

                Vector3 closestPoint = other.ClosestPoint(transform.position);
                Vector3 normal = (transform.position - closestPoint).normalized;

                Vector3 reflectDir = Vector3.Reflect(dir.normalized, normal) * dir.magnitude;
                reflectDir.y = 0;
                _trajectory.ShootDirInit(reflectDir);
                
                _currentReflectionCount++;
            }
            else
                CallDestroyEvent();
        }
    }

    protected void ImpactEffectPlay()
    {
        var effectEvt = SpawnEvents.EffectSpawn;
        effectEvt.effectItem = _impactEffectPoolSO;
        effectEvt.position = transform.position;
        effectEvt.rotation= Quaternion.identity;
        effectEvt.scale = Vector3.one;
        effectEvt.parant = null;
        
        _spawnEventChannelSO.RaiseEvent(effectEvt);
    }

    protected void SoundPlay(SoundSO soundSo)
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.clipData = soundSo;
        soundEvt.position = transform.position;

        _soundEventChannelSO.RaiseEvent(soundEvt);
    }
}
