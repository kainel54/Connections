using System;
using UnityEngine;

[Serializable]
public class DefaultSkillStatElement : BaseSkillStatElement
{
    [SerializeField] private float _defaultValue;
    [HideInInspector] public float currentValue;
    public DefaultSkillStatInfoSO defaultSkillInfo => statInfo as DefaultSkillStatInfoSO;
    
    public float Defaultvalue
    {
        get => _defaultValue;
        set => _defaultValue = value;
    }

    public override void ValueInit()
    {
        currentValue = _defaultValue;
    }
}
