
using UnityEngine;

public class StraightTrajectory : BaseTrajectory
{
    private float _speed;
    ProjectileSkillDataSO _skillData;
    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _skillData = skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        _speed = _skillData.projectileMoveSpeedStat.currentValue;
    }
    public override Vector3 UpdateTrajectory()
    {
        return _shootDir * ((_speed) * Time.deltaTime);
    }
}
