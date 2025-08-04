using UnityEngine;

public class RangeAttackSizeSkillStatSlot : SkillStatBaseSlot
{
    public override void Init(BaseSkillStatElement baseSkillStatElement)
    {
        base.Init(baseSkillStatElement);
        
        RangeAttackSizeSkillStatElement rangeAttackSizeSkillStat = baseSkillStatElement 
            as RangeAttackSizeSkillStatElement;

        switch (rangeAttackSizeSkillStat.attackType)
        {
            case RangeSkillAttackType.Sphere:
                SphereText(rangeAttackSizeSkillStat);
                break;
            case RangeSkillAttackType.Square:
                SquareText(rangeAttackSizeSkillStat);
                break;
        }
    }

    private void SphereText(RangeAttackSizeSkillStatElement rangeAttackSizeSkillStatElement)
    {
        float currentValue = rangeAttackSizeSkillStatElement.currentSphereValue;
        float defaultValue = rangeAttackSizeSkillStatElement.SphereDefaultValue;

        SetText(currentValue, defaultValue);
    }
    
    private void SquareText(RangeAttackSizeSkillStatElement rangeAttackSizeSkillStatElement)
    {
        float currentWidth = rangeAttackSizeSkillStatElement.currentWidthValue;
        float defaultWidth = rangeAttackSizeSkillStatElement.WidthDefaultValue;
        
        float currentHeight = rangeAttackSizeSkillStatElement.currentHeightValue;
        float defaultHeight = rangeAttackSizeSkillStatElement.HeightDefaultValue;
        
        float currentValue = currentWidth * currentHeight;
        float defaultValue = defaultWidth * defaultHeight;

        SetText(currentValue, defaultValue);
    }

    private void SetText(float currentValue, float defaultValue)
    {
        if (Mathf.Approximately(currentValue, defaultValue))
        {
            _valueText.SetText($"{currentValue}");
            return;
        }
        
        float additionalValue = currentValue - defaultValue;
        float absValue = Mathf.Abs(additionalValue);
        
        if(additionalValue > 0)
            _valueText.
                SetText($"{currentValue}  <color=green>+{absValue}</color>");
        else
            _valueText.
                SetText($"{currentValue}  <color=red>-{absValue}</color>");
    }
}
