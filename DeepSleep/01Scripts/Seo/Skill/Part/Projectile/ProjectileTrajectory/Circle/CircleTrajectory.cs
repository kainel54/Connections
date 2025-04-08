using UnityEngine;
using UnityEngine.Splines;

public class CircleTrajectory : BaseTrajectory
{
    private float _speed = 5f;
    private float _rotationSpeed = 5f; // 원형 회전 속도
    private float _radius = 0.1f; // 회전 반경

    private float angle = 0f;
    private Vector3 direction; // 이동 방향

    private SkillProjectileObj _obj;

    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        direction = skillProjectileObj.transform.forward;
        // _radius = (skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO).rangeSize;
        // _speed = (skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO).projMoveSpeed;
        // _rotationSpeed = (skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO).projMoveSpeed;
    }

    public override Vector3 UpdateTrajectory()
    {
        angle += _rotationSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * _radius;
        float y = Mathf.Sin(angle) * _radius;

        Vector3 circularMovement = new Vector3(x, 0, y);
        Vector3 forwardMovement = direction * (_speed * Time.deltaTime);

        Vector3 newPosition = circularMovement + forwardMovement;

        return newPosition;
    }

}
