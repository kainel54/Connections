using TMPro;
using UnityEngine;

public class RangeSkillStatText : SkillStatTextUI
{
    [SerializeField] private TextMeshProUGUI _objCountText;
    [SerializeField] private TextMeshProUGUI _rangeText;
    
    public override void Init(SkillFieldDataSO skillFieldData)
    {
        RangeSkillDataSO rangeSkillData = (RangeSkillDataSO)skillFieldData;
        
        _objCountText.SetText($"생성 개수 :  {rangeSkillData.skillObjCreateCount}개");
        _rangeText.SetText($"범위 :  {rangeSkillData.rangeSize:F1}");
    }
}
