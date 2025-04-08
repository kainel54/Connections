using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YH.Animators;
using YH.Entities;
using YH.Players;

public class PlayerAnimator : MonoBehaviour, IEntityComponent
{

    [SerializeField]
    private AnimParamSO _xVelocityParam, _zVelocityParam, _dashParam, _fireParam, _reloadParam, _reloadSpeedParam,_deathParam;

    private Animator _animator;
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerAttackCompo _playerAttack;

    public event Action<bool> ReloadAnimationStatusChange;
    public event Action OnDieEvent;

    private float _lastAttackTime;
    private bool _isAttackMode;

    public void Initialize(Entity entity)
    {
        _animator = GetComponent<Animator>();
        _player = entity as Player;

        _playerMovement = _player.GetCompo<PlayerMovement>();
        _playerMovement.OnMovementEvent += HandleMovementEvent;
        _playerMovement.OnDashEvent += HandleDashEvent;
        _playerAttack = _player.GetCompo<PlayerAttackCompo>();
        _playerAttack.FireEvent += HandleFireEvent;
        _playerAttack.ReloadEvent += HandleReloadStart;
    }

    public void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < _animator.layerCount; i++)
        {
            float weight = i == layerIndex ? 1 : 0;
            _animator.SetLayerWeight(i, weight);
        }
    }
    private void HandleDashEvent()
    {
        if(_playerMovement.CanDash)
            _animator.SetTrigger(_dashParam.hashValue);
    }

  
    private void HandleFireEvent(int a,int b)
    {
        _animator.SetTrigger(_fireParam.hashValue);
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        float x = Vector3.Dot(movement.normalized, transform.right);
        float z = Vector3.Dot(movement.normalized, transform.forward);

        float dampTime = 0.1f;
        _animator.SetFloat(_xVelocityParam.hashValue, x, dampTime, Time.fixedDeltaTime);
        _animator.SetFloat(_zVelocityParam.hashValue, z, dampTime, Time.fixedDeltaTime);
    }

    private void ReloadAnimationEnd()
    {
        ReloadAnimationStatusChange?.Invoke(false);
    }

    private void HandleReloadStart(float reloadSpeed)
    {
        ReloadAnimationStatusChange?.Invoke(true);
        _animator.SetFloat(_reloadSpeedParam.hashValue, reloadSpeed);
        _animator.SetTrigger(_reloadParam.hashValue);
    }


    public void SetDie()
    {
        _animator.SetTrigger(_deathParam.hashValue);
    }

    public void DeathEvent()
    {
        OnDieEvent?.Invoke();
    }

    public void DashEvent()
    {
        _playerMovement.Dash();
    }

}
