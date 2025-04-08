using UnityEngine;

public class CircleZigzagTrajectory : BaseTrajectory
{
    private float _speed = 5f;
    private float _lifeTime = 10f;

    public float zigzagInterval = 0.5f;  // 방향 변경 주기
    public float zigzagAmount = 2f;  // 한 번에 이동하는 거리

    private int _zigzagDirection = 1; // 이동 방향
    private float _timer;

    private bool _isOneTime = true;
    private float halfZigzagTimer = 0f;

    public float rotationSpeed = 5f; // 원형 회전 속도
    public float radius = 5f; // 회전 반경
    private float angle = 0f;

    private ProjectileSkillDataSO _skillData;

    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _skillData = skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        _speed = _skillData.projMoveSpeed;
        halfZigzagTimer = zigzagInterval * 0.5f;

        zigzagInterval = halfZigzagTimer;
    }

    public override Vector3 UpdateTrajectory()
    {
        _timer += Time.fixedDeltaTime;

        // 특정 시간마다 방향 변경
        if (_timer >= zigzagInterval)
        {
            _zigzagDirection *= -1;
            _timer = 0f;

            if (_isOneTime)
            {
                _isOneTime = false;
                zigzagInterval *= 2;
            }
        }
        // Vector3 centerPos = new Vector3(_zigzagDirection, 0, 1) * zigzagInterval;
        Vector3 zigzagDir = new Vector3(_zigzagDirection, 0, 1);

        angle += rotationSpeed * Time.fixedDeltaTime;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);

        Vector3 dir = zigzagDir + new Vector3(x, 0, y) * radius;

        return dir * _speed * Time.fixedDeltaTime;
    }
}
