using UnityEngine;

public class CriticalSkillStatSlot : SkillStatBaseSlot
{
    public override void Init(BaseSkillStatElement baseSkillStatElement)
    {
        base.Init(baseSkillStatElement);
        DefaultSkillStatElement defaultSkillStatElement = baseSkillStatElement as DefaultSkillStatElement;
        
        float currentValue = defaultSkillStatElement.currentValue;
        float defaultValue = defaultSkillStatElement.Defaultvalue;

        if (Mathf.Approximately(currentValue, defaultValue))
        {
            _valueText.SetText($"{currentValue}%");
            return;
        }
        
        float additionalValue = currentValue - defaultValue;
        float absValue = Mathf.Abs(additionalValue);
        
        if(additionalValue > 0)
            _valueText.
                SetText($"{currentValue}%  <color=green>+{absValue}</color>");
        else
            _valueText.
                SetText($"{currentValue}%  <color=red>-{absValue}</color>");
    }
}
