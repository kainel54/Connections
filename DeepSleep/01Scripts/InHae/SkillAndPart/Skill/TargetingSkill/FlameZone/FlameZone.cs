using UnityEngine;

public class FlameZone : SkillTargetingObj
{
    [SerializeField] private ParticleSystem[] _particles;
    private float _radius;
    private float _duration;

    private SkillDamageCasterParent _skillDamageCasterParent;

    protected override void Awake()
    {
        base.Awake();
        _skillDamageCasterParent = GetComponentInChildren<SkillDamageCasterParent>();
    }

    public override void Initialize(Skill currentSkill, Transform shootTrm)
    {
        base.Initialize(currentSkill, shootTrm);

        _radius = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO)
            .rangeAttackSizeStat.currentSphereValue / 2;
        _duration = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).skillActiveDurationStat.currentValue;

        _damage /= 6;
        
        transform.localScale = new Vector3(_radius, _radius, _radius);
        _skillDamageCasterParent.Init(this, false);
        SetDuration(_duration);
        SelfPoolPush(_duration);
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
        SelfPoolPush();
    }
}
