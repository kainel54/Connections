using TMPro;
using UnityEngine;

public class GenericSkillStatText : SkillStatTextUI
{
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _attackCountText;
    [SerializeField] private TextMeshProUGUI _coolTimeText;
    
    public override void Init(SkillFieldDataSO skillFieldData)
    {
        GenericSkillDataSO genericSkillData = (GenericSkillDataSO)skillFieldData;
        
        _damageText.SetText($"공격력 : {genericSkillData.damage}");
        _attackCountText.SetText($"공격 횟수 : {genericSkillData.attackCount}회");
        _coolTimeText.SetText($"재사용 대기시간 : {genericSkillData.coolTime}초");
    }
}
