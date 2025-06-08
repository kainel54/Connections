using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SkillResultDescription : MonoBehaviour
{
    [SerializeField] private float _fontSize;
    [SerializeField] private string _fontColor;
    
    private TextMeshProUGUI _skillDescription;
    private StringBuilder _resultText;

    private Dictionary<SkillFieldDataType, BaseSkillDescriptionHelper> _skillDescriptionHelpers = new();

    private void Awake()
    {
        _resultText = new StringBuilder();
        
        _skillDescription = GetComponent<TextMeshProUGUI>();

        foreach (var baseSkillDescription in GetComponentsInChildren<BaseSkillDescriptionHelper>())
            _skillDescriptionHelpers.Add(baseSkillDescription.fieldType, baseSkillDescription);
    }

    public void ResultDescription(SkillItemSO dataSo, Skill skill)
    {
        string descriptionText = dataSo.itemDescription;
        _resultText.Clear();

        int statNum = 0;
        for (int i = 0; i < descriptionText.Length; i++)
        {
            if (descriptionText[i] == '[')
            {
                _resultText.Append($"<size={_fontSize}><color={_fontColor}>");
                
                SkillStatInfoSO currentStatInfo = dataSo.skillStats[statNum];
                
                // 숫자가 아닌 특별한 스텟(궤적 같은)들 예외 처리
                bool exceptionStat = false;
                exceptionStat = TrajectoryException(skill, currentStatInfo);
                
                if (exceptionStat)
                {
                    statNum++;
                    _resultText.Append("</color></size>");
                    while (descriptionText[i] != ']')
                        i++;
                    
                    continue;
                }

                char expression = 'a';
                float currentValue = _skillDescriptionHelpers[currentStatInfo.fieldType].ReturnData(currentStatInfo,
                    skill.GetSkillData(currentStatInfo.fieldType));
                
                while (descriptionText[i] != ']')
                {
                    i++;
                    if(descriptionText[i] == '+' || descriptionText[i] == '-'
                            || descriptionText[i] == '*' || descriptionText[i] == '/')
                        expression = descriptionText[i];

                    if (float.TryParse(descriptionText[i].ToString(), out float floatNum))
                        currentValue = ReturnApplyExpression(currentValue, floatNum, expression);
                    else if (int.TryParse(descriptionText[i].ToString(), out int intNum))
                        currentValue = ReturnApplyExpression(currentValue, intNum, expression);
                }
                
                _resultText.Append(currentValue.ToString("F1"));
                _resultText.Append("</color></size>");

                statNum++;
            }
            else
            {
                _resultText.Append(descriptionText[i]);
            }
        }
        _skillDescription.text = _resultText.ToString();
    }

    private bool TrajectoryException(Skill skill, SkillStatInfoSO currentStatInfo)
    {
        if (currentStatInfo.fieldType != SkillFieldDataType.Projectile)
            return false;
        
        ProjectileSkillDataSO projectileSkillDataSo = skill.GetSkillData(SkillFieldDataType.Projectile) 
            as ProjectileSkillDataSO;
        
        if (projectileSkillDataSo.skillStatElements.ContainsKey(currentStatInfo) 
            && projectileSkillDataSo.skillStatElements[currentStatInfo] is TrajectorySkillStatElement trajectory)
        {
            _resultText.Append(TrajectoryTranslateManager.Instance.TrajectoryTranslate(trajectory.currentTrajectory));
            return true;
        }

        return false;
    }

    private float ReturnApplyExpression(float currentNum, float targetNum, char expression)
    {
        float num = currentNum;
        switch (expression)
        {
            case '+':
                num += targetNum;
                break;
            case '-':
                num -= targetNum;
                break;
            case '*':
                num *= targetNum;
                break;
            case '/':
                num /= targetNum;
                break;
        }
        return num;
    }
}
