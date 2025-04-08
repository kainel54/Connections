using UnityEngine;

public abstract class SkillStatTextUI : MonoBehaviour
{
    public SkillFieldDataType fieldType;
    
    public abstract void Init(SkillFieldDataSO skillFieldData);
}
