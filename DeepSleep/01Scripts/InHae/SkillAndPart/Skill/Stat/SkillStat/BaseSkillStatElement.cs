using System;

[Serializable]
public abstract class BaseSkillStatElement
{
    public SkillStatInfoSO statInfo;
    public abstract void ValueInit();
}
