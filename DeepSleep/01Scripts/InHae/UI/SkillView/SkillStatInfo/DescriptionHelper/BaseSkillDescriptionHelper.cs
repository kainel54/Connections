using UnityEngine;

public abstract class BaseSkillDescriptionHelper : MonoBehaviour
{
    public SkillFieldDataType fieldType;
    
    public abstract float ReturnData(SkillStatInfoSO currentStatInfo, SkillFieldDataSO currentFieldData);
}
