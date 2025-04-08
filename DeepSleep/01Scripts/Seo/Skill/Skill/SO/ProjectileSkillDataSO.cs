using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSkillDataSO", menuName = "SO/SkillData/ProjectileSkillDataSO")]
public class ProjectileSkillDataSO : SkillFieldDataSO
{
    public int skillObjCreateCount;
    private int SkillObjCreateCount;

    public float projMoveSpeed;
    private float ProjMoveSpeed;

    public float skillObjCreateDelay;  // ������ ��ų�� ��� �ణ ������ �־����.
    private float SkillObjCreateDelay;  // ������ ��ų�� ��� �ణ ������ �־����.

    public bool ispenetration;  //���� ���ɿ���
    private bool Ispenetration;  //���� ���ɿ���

    public bool canBeHit;
    private bool CanBeHit;

    public List<TrajectoryType> currentTrajectoryList;

    public override void SetDefaultValues()
    {
        SkillObjCreateCount = skillObjCreateCount;
        ProjMoveSpeed = projMoveSpeed;
        SkillObjCreateDelay = skillObjCreateDelay;
        Ispenetration = ispenetration;
        CanBeHit = canBeHit;
    }

    public override void Init()
    {
        skillObjCreateCount = SkillObjCreateCount;
        projMoveSpeed = ProjMoveSpeed;
        skillObjCreateDelay = SkillObjCreateDelay;
        ispenetration = Ispenetration;
        canBeHit = CanBeHit;

        currentTrajectoryList.Clear();
    }
}
