using System;
using System.Collections;
using UnityEngine;
using YH.Core;

public class GroundSlash : SkillRangeObj
{
    private float _width;
    private float _defaultWidth;
    private float _height;
    private float _defaultHeight;

    private RangeSkillDataSO _rangeData;
    private GenericSkillDataSO _genericData;

    private SkillDamageCasterParent _skillDamageCasterParent;
    
    private Coroutine _waitCoroutine;

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

        _rangeData = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO);
        _genericData = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO);

        _width = _rangeData.rangeAttackSizeStat.currentWidthValue;
        _defaultWidth = _rangeData.rangeAttackSizeStat.WidthDefaultValue;
        float widthScale = _width / _defaultWidth;
        widthScale = (float)Math.Round(widthScale, 2);
        
        _height = _rangeData.rangeAttackSizeStat.currentHeightValue;
        _defaultHeight = _rangeData.rangeAttackSizeStat.HeightDefaultValue;
        float heightScale = _height / _defaultHeight;
        heightScale = (float)Math.Round(heightScale, 2);

        transform.localScale = new Vector3(widthScale, heightScale, heightScale);
        //transform.localScale = new Vector3(shootCount, shootCount, shootCount);  << It Can be another Part

        if (_waitCoroutine != null)
            StopCoroutine(_waitCoroutine);
        
        _waitCoroutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(_genericData.skillDamageDelay);
        _skillDamageCasterParent.Init(this, true);
        _skillDamageCasterParent.HitAction += ApplyDamageAction;
        SelfPoolPush(1.5f);
    }
    
    private void ApplyDamageAction(Collider other)
    {
        CameraManager.Instance.ShakeCamera(2, 2, .3f);
    }

    public override void OnPush()
    {
        base.OnPush();
        _skillDamageCasterParent.HitAction -= ApplyDamageAction;
        _skillDamageCasterParent.CasterEnable(false);
    }
}
