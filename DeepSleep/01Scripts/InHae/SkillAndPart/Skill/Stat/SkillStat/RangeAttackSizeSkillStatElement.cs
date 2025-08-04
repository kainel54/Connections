using System;
using UnityEngine;

public enum RangeSkillAttackType
{
    Sphere, Square,
}

[Serializable]
public class RangeAttackSizeSkillStatElement : BaseSkillStatElement
{
    public RangeSkillAttackType attackType;

    [SerializeField] private float _sphreDefaultValue;
    [SerializeField] private float _widthDefaultValue;
    [SerializeField] private float _heightDefaultValue;

    [HideInInspector] public float currentSphereValue;
    [HideInInspector] public float currentWidthValue;
    [HideInInspector] public float currentHeightValue;

    public RangeAttackSizeSkillStatElement(DefaultSkillStatInfoSO defaultSkillStatInfo) : base(defaultSkillStatInfo)
    {
    }

    public float SphereDefaultValue
    {
        get => _sphreDefaultValue;
        set => _sphreDefaultValue = value;
    }

    public float WidthDefaultValue
    {
        get => _widthDefaultValue;
        set => _widthDefaultValue = value;
    }

    public float HeightDefaultValue
    {
        get => _heightDefaultValue;
        set => _heightDefaultValue = value;
    }

    public override void ValueInit()
    {
        currentSphereValue = _sphreDefaultValue;
        currentWidthValue = _widthDefaultValue;
        currentHeightValue = _heightDefaultValue;
    }
}
