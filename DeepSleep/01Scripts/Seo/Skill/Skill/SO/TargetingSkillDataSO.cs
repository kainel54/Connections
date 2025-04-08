using UnityEngine;

[CreateAssetMenu(fileName = "TargetingSkillDataSO", menuName = "SO/SkillData/TargetingSkillDataSO")]
public class TargetingSkillDataSO : SkillFieldDataSO
{
    public float canUseSkillRange;//���� ���� ���� ��ġ����
    private float CanUseSkillRange;//���� ���� ���� ��ġ����
    
    public int skillObjCreateCount;
    private int SkillObjCreateCount;

    public override void SetDefaultValues()
    {
        CanUseSkillRange = canUseSkillRange;
        SkillObjCreateCount = skillObjCreateCount;
    }

    public override void Init()
    {
        canUseSkillRange = CanUseSkillRange;
        skillObjCreateCount = SkillObjCreateCount;
    }
}
