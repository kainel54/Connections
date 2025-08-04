using UnityEngine;

public class SpinningSlashKnife : SkillProjectileObj
{
    private SkillDamageCasterParent _skillDamageCasterParent;
    
    
    protected override void Awake()
    {
        base.Awake();
        _skillDamageCasterParent = GetComponentInChildren<SkillDamageCasterParent>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _skillDamageCasterParent.HitAction -= ApplyDamageAction;
    }

    public override void Initialize(Skill currentSkill, Transform shootTrm)
    {
        base.Initialize(currentSkill, shootTrm);
        
        _damage /= 2;
        
        _skillDamageCasterParent.Init(this, false);
        _skillDamageCasterParent.HitAction += ApplyDamageAction;
    }

    private void ApplyDamageAction(Collider other)
    {
        SoundPlay(_hitSound);
        ImpactEffectPlay();
    }

    public override void OnPop()
    {
        base.OnPop();
        _skillDamageCasterParent.HitAction -= ApplyDamageAction;
    }
}
