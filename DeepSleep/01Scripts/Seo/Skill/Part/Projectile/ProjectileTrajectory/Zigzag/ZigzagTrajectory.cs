using System;
using UnityEngine;

public class ZigzagTrajectory : BaseTrajectory
{
    private ProjectileSkillDataSO _skillData;
    private float _speed;
    private float _zigzagInterval = 0.1f;  // 방향 변경 주기
    private float _zigzagAmount = 2f;  // 한 번에 이동하는 거리

    private int _zigzagDirection = 1; // 이동 방향
    private float _timer;

    private bool _isOneTime = true;

    public override void Init(SkillProjectileObj skillProjectileObj)
    {
        base.Init(skillProjectileObj);
        _skillData = skillProjectileObj.skill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO;
        _speed = _skillData.projMoveSpeed;
    }
    public override Vector3 UpdateTrajectory()
    {
        _timer += Time.deltaTime;

        // 특정 시간마다 방향 변경
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

        Vector3 moveDirection = new Vector3(_zigzagDirection * _zigzagAmount * _speed / 2, 0, _speed) * Time.deltaTime;
        return transform.TransformDirection(moveDirection);
    }
}
