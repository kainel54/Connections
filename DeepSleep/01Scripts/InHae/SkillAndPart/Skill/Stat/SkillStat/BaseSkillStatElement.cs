using System;

[Serializable]
public abstract class BaseSkillStatElement
{
    public BaseSkillStatElement(DefaultSkillStatInfoSO defaultSkillStatInfo)
    {
        this.statInfo = defaultSkillStatInfo;
    }

    public SkillStatInfoSO statInfo;
    public abstract void ValueInit();
}
