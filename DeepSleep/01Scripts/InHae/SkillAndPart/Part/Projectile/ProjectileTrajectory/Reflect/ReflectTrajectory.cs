using UnityEngine;

public class ReflectTrajectory : BaseTrajectory
{
    private Vector3 _direction;
    private float _speed;
    private int _count;

    private ProjectileSkillDataSO _skillData;

    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _skillData = skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        _speed = _skillData.projectileMoveSpeedStat.currentValue;
        _direction = transform.forward;
        _count = 0;

        skillProjectileObj.ReflectEvent += Reflect;
    }

    public void Reflect(Vector3 normal)
    {
        if (_skillData.projectileReflectionCountStat.currentValue >= _count)
        {
            _direction = Vector3.Reflect(_direction, normal);
            _direction.y = 0;
            _direction = _direction.normalized;

            _count += 1;
        }
        else
        {
        }

    }

    public override Vector3 UpdateTrajectory()
    {
        return _direction * _speed * Time.fixedDeltaTime;
    }
}
