using YH.Animators;
using YH.Entities;
using YH.FSM;
using UnityEngine;

namespace YH.Players
{
    public class PlayerMoveState : EntityState
    {
        private float _enterTime;
        private PointNClickPlayer _player;
        private EntityAIMover _mover;
        public PlayerMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as PointNClickPlayer;
            _mover = entity.GetCompo<EntityAIMover>();
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Move");
            _enterTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (!_mover.IsMoving)
            {
                _player.ChangeState(FSMState.Idle);
            }
        }
    }
}
