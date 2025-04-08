
using UnityEngine;

public class StraightTrajectory : BaseTrajectory
{
    private float _speed = 5f;
    ProjectileSkillDataSO _skillData;
    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _skillData = skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        _speed = _skillData.projMoveSpeed;
    }
    public override Vector3 UpdateTrajectory()
    {
        return skillProjectileObj.transform.forward * (_speed) * Time.deltaTime;
    }
}
