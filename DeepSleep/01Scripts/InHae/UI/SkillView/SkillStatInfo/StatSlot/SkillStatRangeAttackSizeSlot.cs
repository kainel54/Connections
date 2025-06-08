public class SkillStatRangeAttackSizeSlot : SkillStatBaseSlot
{
    public override void Init(BaseSkillStatElement baseSkillStatElement)
    {
        base.Init(baseSkillStatElement);
        
        RangeAttackSizeSkillStatElement rangeAttackSizeSkillStat = baseSkillStatElement 
            as RangeAttackSizeSkillStatElement;

        string text = "";
        
        switch (rangeAttackSizeSkillStat.attackType)
        {
            case RangeSkillAttackType.Sphere:
                text = rangeAttackSizeSkillStat.currentSphereValue.ToString("F1");
                break;
            case RangeSkillAttackType.Square:
                text = 
                    $"{(rangeAttackSizeSkillStat.currentWidthValue * rangeAttackSizeSkillStat.currentHeightValue):F1}";
                break;
        }
        
        _valueText.text = text;
    }
}
