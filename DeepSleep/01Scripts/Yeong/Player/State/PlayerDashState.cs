using DG.Tweening;
using YH.Animators;
using YH.Entities;
using YH.FSM;
using UnityEngine;
using YH.Core;
using System;

namespace YH.Players
{
    public class PlayerDashState : EntityState
    {
        private PointNClickPlayer _player;
        private EntityAIMover _mover;
        //private HealthCompo _health;

        private readonly float _dashDistance = 10f, _dashTime = 0.5f;
        public PlayerDashState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as PointNClickPlayer;
            _mover = entity.GetCompo<EntityAIMover>();
        }

        public override void Enter()
        {
            base.Enter();

            Vector3 rollingDirection = GetRollingDirection();
            _player.transform.rotation = Quaternion.LookRotation(rollingDirection);

            _mover.CanManualMove = false;
            _mover.StopImmediately();
            Vector3 destination = _player.transform.position + _player.transform.forward.normalized * (_dashDistance - 0.5f);

            _player.transform.DOMove(destination, _dashTime).SetEase(Ease.OutQuad).OnComplete(EndDash);
        }


        private Vector3 GetRollingDirection()
        {
            Vector3 direction = Vector3.zero;
            Vector3 moveInput = _player.PlayerInput.Movement;
            if (_player.PlayerInput.Movement.magnitude < 0.1f)
            {
                moveInput = _player.transform.forward.normalized;
                moveInput = new Vector2(moveInput.x, moveInput.z);
            }

            direction = Quaternion.Euler(0, -45f, 0) * new Vector3(moveInput.x, 0, moveInput.y);
            //_targetRotation = Quaternion.LookRotation(direction);
            return direction;
        }


        public override void Exit()
        {
            Vector3 endPos = new Vector3(_player.transform.position.x, 0, _player.transform.position.z);
            _mover.EndPos = endPos;
            _mover.StopImmediately();
            _mover.CanManualMove = true;
            base.Exit();
        }

        private void EndDash()
        {
            _player.ChangeState(FSMState.Idle);
        }


    }
}
