public class SkillStatDefaultSlot : SkillStatBaseSlot
{
    public override void Init(BaseSkillStatElement baseSkillStatElement)
    {
        base.Init(baseSkillStatElement);
        
        DefaultSkillStatElement defaultSkillStatElement = baseSkillStatElement as DefaultSkillStatElement;
        _valueText.SetText(defaultSkillStatElement.currentValue.ToString());
    }
}
