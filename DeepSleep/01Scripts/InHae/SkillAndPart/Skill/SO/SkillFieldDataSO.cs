using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum SkillFieldDataType
{
    Range,
    Projectile,
    Generic,
    Targeting,
}

public abstract class SkillFieldDataSO : ScriptableObject
{
    public SkillFieldDataType fieldType;
    public Dictionary<SkillStatInfoSO, BaseSkillStatElement> skillStatElements;
    
    public virtual void Init()
    {
        skillStatElements = new Dictionary<SkillStatInfoSO, BaseSkillStatElement>();
        
        var skillElementInfos = GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => typeof(BaseSkillStatElement).IsAssignableFrom(x.FieldType));

        foreach (var skillElement in skillElementInfos)
        {
            BaseSkillStatElement skillStatElement = (BaseSkillStatElement)skillElement.GetValue(this);
            if(skillStatElement.statInfo == null)
                continue;
            
            skillStatElements.Add(skillStatElement.statInfo, skillStatElement);
        }
    }

    public virtual void ValueInit()
    {
        foreach (var skillStatElement in skillStatElements.Values)
            skillStatElement.ValueInit();
    }

    public virtual void Setup(List<DefaultSkillStatInfoSO> defaultSkillStatInfoSOs)
    {
        int index = 0;

        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(DefaultSkillStatElement))
            {
                if (index >= defaultSkillStatInfoSOs.Count)
                {
                    Debug.Log("finish");
                    break;
                }

                var value = new DefaultSkillStatElement(defaultSkillStatInfoSOs[index]);
                field.SetValue(this, value);
                index++;
            }

            if (field.FieldType == typeof(TrajectorySkillStatElement))
            {
                if (index >= defaultSkillStatInfoSOs.Count)
                {
                    Debug.Log("finish");
                    break;
                }

                var value = new TrajectorySkillStatElement(defaultSkillStatInfoSOs[index]);
                field.SetValue(this, value);
                index++;
            }

            if (field.FieldType == typeof(RangeAttackSizeSkillStatElement))
            {
                if (index >= defaultSkillStatInfoSOs.Count)
                {
                    Debug.Log("finish");
                    break;
                }

                var value = new RangeAttackSizeSkillStatElement(defaultSkillStatInfoSOs[index]);
                field.SetValue(this, value);
                index++;
            }
        }
    }
}
