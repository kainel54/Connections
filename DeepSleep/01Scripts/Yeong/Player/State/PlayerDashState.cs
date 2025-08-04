using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;
using YH.Core;

namespace YH.Players
{
    public class PlayerDashState : EntityState
    {
        private readonly float _dashDistance = 8f;
        private readonly float _dashDuration = 0.5f;

        private MOBAPlayer _player;
        private EntityAIMover _mover;
        private EntityHealth _health;

        private Vector3 _dashDirection;
        private float _elapsedTime;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        public PlayerDashState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as MOBAPlayer;
            _mover = entity.GetCompo<EntityAIMover>();
            _health = entity.GetCompo<EntityHealth>();
        }

        public override void Enter()
        {
            base.Enter();

            _player.SetDashing(true);
            _player.PlayerInput.MoveEvent += HandleMoveEvent;

            _mover.CanManualMove = false;
            _mover.CanManualRotate = false;
            _mover.StopImmediately();
            
            _health.SetInvincible(true);

            _dashDirection = GetDashDir();
            _dashDirection.y = 0f;
            _dashDirection.Normalize();

            _mover.RotateToDirection(_dashDirection);

            _startPosition = _player.transform.position;

            Ray ray = new Ray(_startPosition, _dashDirection);
            RaycastHit hit;
            float adjustedDashDistance = _dashDistance;

            if (Physics.Raycast(ray, out hit, _dashDistance, _player.WhatisWall))
            {
                adjustedDashDistance = hit.distance - 0.1f;
                adjustedDashDistance = Mathf.Max(adjustedDashDistance, 0f);
            }

            _targetPosition = _startPosition + _dashDirection * adjustedDashDistance;


            _elapsedTime = 0f;
        }

        private void HandleMoveEvent(bool isClick)
        {
            _player.isMoveClick = isClick;
            _player.MoveEvent?.Invoke();
        }

        public override void Update()
        {
            base.Update();
            _elapsedTime += Time.deltaTime;

            float time = Mathf.Clamp01(_elapsedTime / _dashDuration);
            Vector3 newPos = Vector3.Lerp(_startPosition, _targetPosition, time);
            _player.transform.position = newPos;

            if (time >= 1f)
            {
                EndDash();
            }
        }

        private Vector3 GetDashDir()
        {
            Vector3 mousePosition = _player.PlayerInput.GetWorldMousePosition();
            Vector3 dir = (mousePosition - _player.transform.position);
            dir.y = 0f;
            return dir.normalized;
        }

        private void EndDash()
        {
            _mover.RotateToDirection(_dashDirection);
            _health.SetInvincible(false);
            _player.ChangeState(FSMState.Idle);
        }

        public override void Exit()
        {
            _player.SetDashing(false);
            _player.PlayerInput.MoveEvent -= HandleMoveEvent;

            _mover.EndPos = new Vector3(_player.transform.position.x, 0, _player.transform.position.z);
            _mover.StopImmediately();
            _mover.CanManualMove = true;
            _mover.CanManualRotate = true;

            base.Exit();
        }
    }
}
