using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SkillResultDescription : MonoBehaviour
{
    [SerializeField] private float _fontSize;
    [SerializeField] private Color _fontColor;
    
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

    public void Init()
    {
        _skillDescription.text = string.Empty;
    }

    public void ResultDescription(SkillItemSO dataSo, Skill skill)
    {
        string descriptionText = dataSo.itemDescription;
        _resultText.Clear();

        int statIndex = 0;

        for (int i = 0; i < descriptionText.Length; i++)
        {
            if (descriptionText[i] == '[')
            {
                if (statIndex >= dataSo.descriptionUseSkillStat.Count)
                    break;

                _resultText.Append($"<size={_fontSize}><color=#{ColorUtility.ToHtmlStringRGB(_fontColor)}>");

                SkillStatInfoSO currentStatInfo = dataSo.descriptionUseSkillStat[statIndex];

                if (HandleTrajectoryException(skill, currentStatInfo))
                {
                    statIndex++;
                    SkipUntil(ref i, descriptionText, ']');
                    _resultText.Append("</color></size>");
                    continue;
                }

                float currentValue = _skillDescriptionHelpers[currentStatInfo.fieldType]
                    .ReturnData(currentStatInfo, skill.GetSkillData(currentStatInfo.fieldType));
                
                i++;
                char expression = '+';
                string numberBuffer = string.Empty;

                while (i < descriptionText.Length && descriptionText[i] != ']')
                {
                    char currentChar = descriptionText[i];

                    if (currentChar == '+' || currentChar == '-' || currentChar == '*' || currentChar == '/')
                    {
                        if (!string.IsNullOrEmpty(numberBuffer) && float.TryParse(numberBuffer, out float parsedNum))
                        {
                            currentValue = ApplyExpression(currentValue, parsedNum, expression);
                            numberBuffer = string.Empty;
                        }
                        expression = currentChar;
                    }
                    else if (char.IsDigit(currentChar) || currentChar == '.')
                    {
                        numberBuffer += currentChar;
                    }

                    i++;
                }

                if (!string.IsNullOrEmpty(numberBuffer) && float.TryParse(numberBuffer, out float lastNum))
                {
                    currentValue = ApplyExpression(currentValue, lastNum, expression);
                }

                _resultText.Append(currentValue.ToString("F1"));
                _resultText.Append("</color></size>");

                statIndex++;
            }
            else
            {
                _resultText.Append(descriptionText[i]);
            }
        }

        _skillDescription.text = _resultText.ToString();
    }

    private void SkipUntil(ref int index, string text, char targetChar)
    {
        while (index < text.Length && text[index] != targetChar)
            index++;
    }

    private bool HandleTrajectoryException(Skill skill, SkillStatInfoSO statInfo)
    {
        if (statInfo.fieldType != SkillFieldDataType.Projectile)
            return false;

        if (skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO projectileData &&
            projectileData.skillStatElements.TryGetValue(statInfo, out var element) &&
            element is TrajectorySkillStatElement trajectory)
        {
            _resultText.Append(EnumStringManager.Instance.GetString(trajectory.currentTrajectory));
            return true;
        }

        return false;
    }

    private float ApplyExpression(float currentNum, float targetNum, char expression)
    {
        return expression switch
        {
            '+' => currentNum + targetNum,
            '-' => currentNum - targetNum,
            '*' => currentNum * targetNum,
            '/' => targetNum != 0 ? currentNum / targetNum : 0f,
            _ => currentNum
        };
    }
}
