using UnityEngine;

public class CircleTrajectory : BaseTrajectory
{
    [SerializeField] private float _radius = 0.1f;
    private float _speed;
    private float _angle;

    private SkillProjectileObj _obj;
    private Vector3 _prevCircularOffset;
    private Vector3 _centerPosition;

    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _speed = (skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO)
            .projectileMoveSpeedStat.currentValue;

        _centerPosition = skillProjectileObj.transform.position;
        _centerPosition.y = 0f;

        _angle = 0f;
        _prevCircularOffset = _centerPosition;
    }

    public override Vector3 UpdateTrajectory()
    {
        Vector3 forwardMovement = _shootDir * (_speed * Time.fixedDeltaTime);
        _centerPosition += forwardMovement;

        _angle += _speed * Time.fixedDeltaTime;

        float x = Mathf.Cos(_angle) * _radius;
        float z = Mathf.Sin(_angle) * _radius;
        Vector3 currentCircularPosition = _centerPosition + new Vector3(x, 0f, z);

        Vector3 circularDelta = currentCircularPosition - _prevCircularOffset;
        _prevCircularOffset = currentCircularPosition;

        return circularDelta;
    }
}