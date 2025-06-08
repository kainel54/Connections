using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Players
{
    public class PlayerAttackState : EntityState
    {
        private PointNClickPlayer _player;
        private EntityAIMover _mover;
        private PlayerAnimatorTrigger _animationTrigger;
        public PlayerAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as PointNClickPlayer;
            _mover = _player.GetCompo<EntityAIMover>();
            _animationTrigger = _player.GetCompo<PlayerAnimatorTrigger>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();

            Vector3 toward = _player.target.position - _player.transform.position;

            toward.y = 0;
            _player.transform.rotation
                = Quaternion.LookRotation(toward);

            _animationTrigger.OnAnimationEndTrigger += HandleAnimationEndTrigger;
        }

        public override void Exit()
        {
            base.Exit();
            _animationTrigger.OnAnimationEndTrigger -= HandleAnimationEndTrigger;
            _player.lastAttackTime = Time.time;
        }

        private void HandleAnimationEndTrigger()
        {
            _player.ChangeState(FSMState.Idle);
        }
    }
}


