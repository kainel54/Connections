using System;
using UnityEngine;

public class ZigzagTrajectory : BaseTrajectory
{
    private ProjectileSkillDataSO _skillData;
    private float _speed;
    [SerializeField] private float _zigzagInterval = 0.1f;  // 방향 변경 주기
    [SerializeField] private float _zigzagAmount = 5f;  // 한 번에 이동하는 거리

    private int _zigzagDirection = 1; // 이동 방향
    private float _timer;

    private bool _isOneTime = true;

    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _skillData = skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        _speed = _skillData.projectileMoveSpeedStat.currentValue;
    }
    public override Vector3 UpdateTrajectory()
    {
        _timer += Time.deltaTime;

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
        Vector3 zigzagDir = _shootDir * _speed + right * (_zigzagDirection * _zigzagAmount);
        return zigzagDir * Time.fixedDeltaTime;
    }
}
