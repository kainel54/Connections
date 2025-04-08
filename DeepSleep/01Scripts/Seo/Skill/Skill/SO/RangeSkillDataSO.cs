using UnityEngine;

[CreateAssetMenu(fileName = "RangeSkillDataSO", menuName = "SO/SkillData/RangeSkillDataSO")]
public class RangeSkillDataSO : SkillFieldDataSO
{
    public int skillObjCreateCount;  // it might be have to go genericskillData
    private int SkillObjCreateCount;

    //here is for sphere cast
    public float rangeSize;
    private float RangeSize;

    //here is for box cast
    public float width;
    private float Width;
    
    public float height; // front
    private float Height;
    
    public override void SetDefaultValues()
    {
        SkillObjCreateCount = skillObjCreateCount;
        RangeSize = rangeSize;
        Width = width;
        Height = height;
    }

    public override void Init()
    {
        skillObjCreateCount = SkillObjCreateCount;
        rangeSize = RangeSize;
        width = Width;
        height = Height;
    }
}
