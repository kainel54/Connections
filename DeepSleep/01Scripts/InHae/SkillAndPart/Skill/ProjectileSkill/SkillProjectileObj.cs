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
    
    private ProjectileSkillDataSO _projectileSkillData;
    
    private TrajectoryManager _trajectoryManager;
    private BaseTrajectory _trajectory;

    private bool _canBeHit;

    private Vector3 _dir;

    private int _reflectionCount;
    private int _currentReflectionCount;
    
    private int _penetrationCount;
    private int _currentPenetrationCount;
    
    private Coroutine _lifeTimeCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _trajectoryManager = GetComponentInChildren<TrajectoryManager>();
        OnSkillEndEvent += HandleProjectileEnd;
    }

    protected virtual void HandleProjectileEnd()
    {
        ImpactEffectPlay();
    }

    public override void Initialize(Skill currentSkill, Transform shootTrm)
    {
        base.Initialize(currentSkill, shootTrm);

        _projectileSkillData = skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        
        _canBeHit = _projectileSkillData.canBeHit;

        _reflectionCount = (int)_projectileSkillData.projectileReflectionCountStat.currentValue;
        _currentReflectionCount = 0;
        
        _penetrationCount = (int)_projectileSkillData.projectilePenetrationCountStat.currentValue;
        _currentPenetrationCount = 0;
        
        SetTrajectory();

        SelfPoolPush(_lifetime);
    }

    private void SetTrajectory()
    {
        _trajectory = _trajectoryManager.
            GetTrajectory(_projectileSkillData.projectileTrajectoryStat.currentTrajectory);
        _trajectory.Init(this);
    }

    protected virtual void FixedUpdate()
    {
        if(!_isInited)
            return;
        
        _dir = _trajectory.UpdateTrajectory();
        transform.forward = _dir;
        transform.position += _dir;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        BigBigCheck(other);
        PenetrationCheck(other);
        ReflectCheck(other);
    }

    private void BigBigCheck(Collider other)
    {
        if (!other.gameObject.CompareTag("Bullet") || !_canBeHit)
            return;
        
        if(TryGetComponent(out IApplyAbleBigBigPart ableBigBigPart))
            ableBigBigPart.ApplyBigBigPart();
    }

    private void PenetrationCheck(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Tower"))
            return;
        
        if (_penetrationCount <= _currentPenetrationCount)
            SelfPoolPush();
        else
            _currentPenetrationCount++;
    }

    private void ReflectCheck(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Wall") 
            && other.gameObject.layer != LayerMask.NameToLayer("Obstacle")
            && !other.CompareTag("Barrier"))
            return;

        if (_reflectionCount > _currentReflectionCount)
        {
            ImpactEffectPlay();

            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Vector3 normal = (transform.position - closestPoint).normalized;

            Vector3 reflectDir = Vector3.Reflect(_dir.normalized, normal) * _dir.magnitude;
            reflectDir.y = 0;
            _trajectory.ShootDirInit(reflectDir);

            _currentReflectionCount++;
        }
        else
            SelfPoolPush();
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
