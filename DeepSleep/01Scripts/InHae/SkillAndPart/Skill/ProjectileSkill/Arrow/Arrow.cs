using UnityEngine;
using YH.Combat;

public class Arrow : SkillProjectileObj
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
        
        _skillDamageCasterParent.Init(this, false);
        _skillDamageCasterParent.HitAction += ApplyDamageAction;
    }
    
    private void ApplyDamageAction(Collider other)
    {
        ImpactEffectPlay();
    }

    public override void OnPush()
    {
        base.OnPush();
        _skillDamageCasterParent.HitAction -= ApplyDamageAction;
    }
}
