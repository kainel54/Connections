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

    public float DefaultSphereValue
    {
        get => _sphreDefaultValue;
        set => _sphreDefaultValue = value;
    }

    public float DefaultWidthValue
    {
        get => _widthDefaultValue;
        set => _widthDefaultValue = value;
    }

    public float DefaultHeightValue
    {
        get => _widthDefaultValue;
        set => _widthDefaultValue = value;
    }

    public override void ValueInit()
    {
        currentSphereValue = _sphreDefaultValue;
        currentWidthValue = _widthDefaultValue;
        currentHeightValue = _heightDefaultValue;
    }
}
