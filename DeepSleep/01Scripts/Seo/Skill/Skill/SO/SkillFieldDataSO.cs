using System;
using UnityEngine;

public enum SkillFieldDataType
{
    Range,
    Projectile,
    Generic,
    Targeting,
}

public abstract class SkillFieldDataSO : ScriptableObject
{
    public SkillFieldDataType fieldType;
    
    public abstract void SetDefaultValues();
    public abstract void Init();
}
