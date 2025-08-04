using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;
using YH.StatSystem;

namespace YH.Players
{
    public class PlayerAttackState : EntityState
    {
        private MOBAPlayer _player;
        private EntityAIMover _mover;
        private PlayerAnimatorTrigger _animationTrigger;
        private PlayerDamageCaster _damageCaster;
        private StatElement _attackSpeedStat;

        private float _lastAttackTime;
        private float _attackComboDelay = 1f;
        private int _attackCombo;
        private bool _isAttackEnable;

        public PlayerAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as MOBAPlayer;
            _attackSpeedStat = _player.GetCompo<EntityStat>().GetElement("AttackSpeed");
            
            _mover = _player.GetCompo<EntityAIMover>();
            _animationTrigger = _player.GetCompo<PlayerAnimatorTrigger>();
            _damageCaster = _player.GetCompo<PlayerDamageCaster>();
        }

        public override void Enter()
        {
            base.Enter();
            if (_attackCombo > 3 || Time.time >= _lastAttackTime + _attackComboDelay)
            {
                _attackCombo = 0;
            }
            _isAttackEnable = false;

            _player.PlayerInput.MoveEvent += HandleMoveEvent;
            _player.AttackEvent?.Invoke();

            _player.AttackComboEvent?.Invoke(_attackCombo);
            
            _renderer.SetSpeed(_attackSpeedStat.Value);
            _renderer.Play($"AttackCombo{_attackCombo}");
            
            _mover.StopImmediately();
            _mover.RotateToDirection((_player.PlayerInput.GetWorldMousePosition() - _player.transform.position).normalized);
            
            _animationTrigger.OnAnimationEndTrigger += HandleAnimationEndTrigger;
            _animationTrigger.OnAttackTrigger += HandleAttackTrigger;
            _animationTrigger.OnMoveForwardTrigger += HandleMoveTrigger;
            _animationTrigger.OnAttackEnableTrigger += HandleAttackEnableTrigger;
            _animationTrigger.OnMoveYTrigger += HandleMoveYTrigger;
            _player.PlayerInput.DashEvent += HandleChangeDashTrigger;
        }

        private void HandleMoveEvent(bool isClick)
        {
            _player.isMoveClick = isClick;
            _player.MoveEvent?.Invoke();
        }

        private void HandleMoveYTrigger(float yDirection)
        {
            _mover.ApplyVerticalOffset(yDirection, _player.moveYTime[0]);
        }

        private void HandleMoveTrigger(float fowardDirection)
        {
            _mover.MoveForward(fowardDirection, _player.moveForwardTime[_attackCombo]);
        }

        private void HandleAttackTrigger(bool isActive)
        {
            _damageCaster.SetDamageCaster(isActive);
        }

        public override void Exit()
        {
            base.Exit();

            _renderer.SetSpeed(1f);
            _renderer.Play("Idle");

            _animationTrigger.OnAnimationEndTrigger -= HandleAnimationEndTrigger;
            _animationTrigger.OnAttackTrigger -= HandleAttackTrigger; 
            _animationTrigger.OnMoveForwardTrigger -= HandleMoveTrigger;
            _animationTrigger.OnMoveYTrigger -= HandleMoveYTrigger;
            _animationTrigger.OnAttackEnableTrigger -= HandleAttackEnableTrigger;
            _player.PlayerInput.DashEvent -= HandleChangeDashTrigger;
            _player.PlayerInput.MoveEvent -= HandleMoveEvent;
        }

        private void HandleAttackEnableTrigger()
        {
            _lastAttackTime = Time.time;
            _attackCombo++;
            _isAttackEnable = true;
        }

        private void HandleChangeDashTrigger()
        {
            if (_player.isDashing) return;
            if (_player.currentDashCount <= 0) return;
            _player.DashEvent?.Invoke();
            _player.currentDashCount--;
            _player.ChangeState(FSMState.Dash);
        }

        
        private void HandleAnimationEndTrigger()
        {
            _player.ChangeState(FSMState.Idle);
        }

        public override void Update()
        {
            base.Update();

            if (_player.isAttackClick && _isAttackEnable)
            {
                _player.ChangeState(FSMState.Attack);
            }
        }
    }
}