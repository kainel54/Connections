using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MagicSphere : SkillProjectileObj, IApplyAbleBigBigPart
{
    [SerializeField] private float _damageInterval = 1;
    [SerializeField] private List<Transform> _particles;
    private Dictionary<Transform, Vector3> _particleDefaultScale = new();
    
    private SkillDamageCasterParent _skillDamageCasterParent;

    protected override void Awake()
    {
        base.Awake();
        _skillDamageCasterParent = GetComponentInChildren<SkillDamageCasterParent>();
        foreach (var particle in _particles)
            _particleDefaultScale.Add(particle, particle.localScale);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _skillDamageCasterParent.HitAction -= ApplyDamageAction;
    }

    public override void Initialize(Skill currentSkill, Transform shootTrm)
    {
        base.Initialize(currentSkill, shootTrm);
        
        _skillDamageCasterParent.Init(this, false);
        _skillDamageCasterParent.HitAction += ApplyDamageAction;
    }

    private void ApplyDamageAction(Collider other)
    {
        SoundPlay(_hitSound);
        ImpactEffectPlay();
    }

    public void ApplyBigBigPart()
    {
        foreach (Transform particle in _particles)
        {
            particle.DOScale(particle.localScale * 1.05f, 0.3f);
        }

        _damage += 0.5f; // todo: 중첩 배수 넣기
    }

    public void InitBigBigPart()
    {
        foreach (var particle in _particleDefaultScale)
            particle.Key.transform.localScale = particle.Value;
    }

    public override void OnPop()
    {
        base.OnPop();
        InitBigBigPart();
        _skillDamageCasterParent.HitAction -= ApplyDamageAction;
    }
}