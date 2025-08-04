using UnityEngine;

public class CoolTimeSkillStatSlot : SkillStatBaseSlot
{
    public override void Init(BaseSkillStatElement baseSkillStatElement)
    {
        base.Init(baseSkillStatElement);
        DefaultSkillStatElement defaultSkillStatElement = baseSkillStatElement as DefaultSkillStatElement;
        
        float currentValue = defaultSkillStatElement.currentValue;
        float defaultValue = defaultSkillStatElement.Defaultvalue;

        if (Mathf.Approximately(currentValue, defaultValue))
        {
            _valueText.SetText($"{currentValue}초");
            return;
        }
        
        float additionalValue = currentValue - defaultValue;
        float absValue = Mathf.Abs(additionalValue);
        
        if(additionalValue < 0)
            _valueText.SetText($"{currentValue}초  <color=green>-{absValue}</color>");
        else
            _valueText.SetText($"{currentValue}초  <color=red>+{absValue}</color>");
    }
}
