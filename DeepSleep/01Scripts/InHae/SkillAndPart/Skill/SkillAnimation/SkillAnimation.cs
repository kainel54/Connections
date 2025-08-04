using System;
using System.Collections.Generic;
using UnityEngine;
using YH.Entities;
using YH.FSM;
using YH.Players;

public class SkillAnimation : MonoBehaviour
{
    [SerializeField] protected AnimationClip _animationClip;
    public bool isPlayAnimation = true;
    
    protected Skill _skill;
    protected MOBAPlayer _player;
    
    protected EntityRenderer _playerAnimator;
    protected PlayerAnimatorTrigger _playerAnimatorTrigger;
    protected EntityAIMover _playerMover;

    private static bool _isSkilling = false;

    private bool _isDashEnable;
    
    protected List<Action> _skillActions = new List<Action>() {null, null, null, null};
    
    public virtual void Init(Skill skill)
    {
        _player = skill.player as MOBAPlayer;
        
        _playerAnimator = _player.GetCompo<EntityRenderer>();
        _playerAnimatorTrigger = _player.GetCompo<PlayerAnimatorTrigger>();
        _playerMover = _player.GetCompo<EntityAIMover>();
        
        _skill = skill;
    }

    protected virtual void OnDestroy()
    {
        _player.PlayerInput.DashEvent -= HandlePlaySkillDashEvent;
        _player.PlayerInput.DashEvent -= DashEvent;
        
        _playerAnimatorTrigger.OnAnimationEndTrigger -= HandleAnimationEnd;
        _playerAnimatorTrigger.OnSkillActiveTrigger -= UseSkill;
        _playerAnimatorTrigger.OnDashActiveTrigger -= HandleDashActive;
    }
    
    private void HandlePlaySkillDashEvent()
    {
        SkillCompleteProcess();
        
        _playerAnimator.Play("Idle");
    }

    private void HandleAnimationEnd()
    {
        SkillCompleteProcess();

        _player.ChangeState(FSMState.Idle);
    }

    protected virtual void SkillCompleteProcess()
    {
        _playerMover.CanManualMove = true;
        _player.IsPlaySkill = false;
        _player.IsDashEnable = true;

        _player.PlayerInput.DashEvent -= HandlePlaySkillDashEvent;
        _player.PlayerInput.DashEvent -= DashEvent;
        
        _playerAnimatorTrigger.OnSkillActiveTrigger -= UseSkill;
        _playerAnimatorTrigger.OnAnimationEndTrigger -= HandleAnimationEnd;
        _playerAnimatorTrigger.OnDashActiveTrigger -= HandleDashActive;

        _isSkilling = false;
    }

    public void CheckPlaySkillAnimation()
    {
        if(!CanSkillUseCheck() || _isSkilling || _player.isDashing)
            return;

        if (_skill.GetShootCount() <= 0)
            _skill.SetCoolTime();

        _player.isAttackClick = false;
        
        _isSkilling = true;
        _skill.InputSkillProcess();
        
        if (!isPlayAnimation || _animationClip == null)
        {
            _skill.UseBeforeProcess();
            _isSkilling = false;
        }
        else
        {
            AnimationPlay();
        }
        
        _playerMover.RotateToDirection((_player.PlayerInput.GetWorldMousePosition() - _player.transform.position).normalized);
    }

    protected virtual void AnimationPlay()
    {
        _skillActions[0] = _skill.UseBeforeProcess;
        
        _playerMover.CanManualMove = false;
        _player.ChangeState(FSMState.Idle);

        _player.PlayerInput.StopEvent = null;
        _player.PlayerInput.MoveEvent = null;
        _player.IsPlaySkill = true;

        _player.PlayerInput.DashEvent += HandlePlaySkillDashEvent;
        
        _playerAnimatorTrigger.OnAnimationEndTrigger += HandleAnimationEnd;
        _playerAnimatorTrigger.OnSkillActiveTrigger += UseSkill;
        _playerAnimatorTrigger.OnDashActiveTrigger += HandleDashActive;
            
        _playerAnimator.Play(_animationClip);
    }

    private bool CanSkillUseCheck()
    {
        // 지뢰 등 처리
        if (_skill.GetShootCount() > 0 && !_skill.CanShootSkill)
            return false;
        // 기본 처리
        if(_skill.GetShootCount() <= 1 && _skill.IsSkillCoolTime)
            return false;
        
        return true;
    }

    private void UseSkill(int number)
    {
        _skillActions[number]?.Invoke();
    }
    
    private void HandleDashActive(int isEnable)
    {
        _player.IsDashEnable = isEnable == 1;
        if (_player.IsDashEnable)
        {
            _player.PlayerInput.DashEvent += DashEvent;
            _player.PlayerInput.DashEvent += HandlePlaySkillDashEvent;
        }
        else
        {
            _player.PlayerInput.DashEvent = null;
        }
    }
    
    private void DashEvent()
    {
        if (_player.isDashing || !_player.IsDashEnable) return;
        if (_player.currentDashCount <= 0) return;
        _player.DashEvent?.Invoke();
        _player.currentDashCount--;
        _player.ChangeState(FSMState.Dash);
    }
}
