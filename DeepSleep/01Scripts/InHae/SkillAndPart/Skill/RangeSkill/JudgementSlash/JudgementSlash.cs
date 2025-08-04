using System;
using UnityEngine;
using YH.Core;

public class JudgementSlash : SkillRangeObj
{
    [SerializeField] private float _knockBackForce;
    private float _size;
    private float _defaultSize;

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
        
        _size = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO)
            .rangeAttackSizeStat.currentSphereValue;
        _defaultSize = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO)
            .rangeAttackSizeStat.SphereDefaultValue;
        
        float scale = _size / _defaultSize;
        scale = (float)Math.Round(scale, 2);
        transform.localScale = new Vector3(scale, scale, scale);
        
        Vector3 playerAngleSet = transform.forward;
        transform.position += playerAngleSet;
        
        //transform.position = transform.position + transform.forward * ((shootCount - 1) * _radius);
        
        _skillDamageCasterParent.Init(this, true);
        _skillDamageCasterParent.HitAction += ApplyDamageAction;
        SelfPoolPush(1f);
    }
    
    private void ApplyDamageAction(Collider other)
    {
        CameraManager.Instance.ShakeCamera(1, 1, .3f);
        if (!other.TryGetComponent(out IKnockBackable knockBackAble)) 
            return;
        
        Vector3 normal = (transform.position - other.transform.position).normalized;
        Vector3 point =  other.ClosestPoint(transform.position);
        knockBackAble.KnockBack(normal * -_knockBackForce, point);
    }

    public override void OnPush()
    {
        base.OnPush();
        _skillDamageCasterParent.HitAction -= ApplyDamageAction;
    }
}
