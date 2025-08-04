using YH.Animators;
using YH.Entities;
using YH.FSM;
using UnityEngine;

namespace YH.Players
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
        }

        public override void Update()
        {
            base.Update();

            if (_mover.IsMoving)
            {
                _player.ChangeState(FSMState.Move);
            }
        }
    }
}
