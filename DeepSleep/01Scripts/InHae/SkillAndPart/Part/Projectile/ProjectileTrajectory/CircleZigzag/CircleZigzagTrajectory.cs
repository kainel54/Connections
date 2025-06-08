using UnityEngine;

public class CircleZigzagTrajectory : BaseTrajectory
{
    [Header("Zigzag Settings")]
    [SerializeField] private float _zigzagAmount = 1f;
    [SerializeField] private float _zigzagInterval = 0.5f;
    private int _zigzagDirection = 1;
    private bool _isOneTime;
    private float _timer;

    [Header("Circle Settings")]
    [SerializeField] private float _radius = 0.5f;
    private Vector3 _centerPosition;
    private Vector3 _prevCircularOffset;
    private float _angle;

    private float _speed;
    
    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _speed = (skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO)
            .projectileMoveSpeedStat.currentValue;

        _centerPosition = skillProjectileObj.transform.position;
        _centerPosition.y = 0f;

        _angle = 0f;
        _timer = 0f;
        _zigzagDirection = 1;
        _prevCircularOffset = _centerPosition;
    }

    public override Vector3 UpdateTrajectory()
    {
        _timer += Time.fixedDeltaTime;

        if (_timer >= _zigzagInterval)
        {
            _zigzagDirection *= -1;
            _timer = 0f;

            if (_isOneTime)
            {
                _isOneTime = false;
                _zigzagInterval *= 2;
            }
        }

        Vector3 right = new Vector3(_shootDir.z, 0, -_shootDir.x);
        Vector3 zigzagOffset = _shootDir * _speed + right * (_zigzagDirection * _zigzagAmount);
        _centerPosition += zigzagOffset * Time.fixedDeltaTime;

        _angle += _speed * Time.fixedDeltaTime;
        float x = Mathf.Cos(_angle) * _radius;
        float z = Mathf.Sin(_angle) * _radius;
        Vector3 currentCircularPosition = _centerPosition + new Vector3(x, 0f, z);

        Vector3 circularDelta = currentCircularPosition - _prevCircularOffset;
        _prevCircularOffset = currentCircularPosition;

        return circularDelta;
    }
}

