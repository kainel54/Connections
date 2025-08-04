using YH.Animators;
using YH.Entities;
using YH.FSM;
using UnityEngine;

namespace YH.Players
{
    public class PlayerMoveState : PlayerGroundState
    {

        public PlayerMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            
        }
        public override void Update()
        {
            base.Update();

            if (!_mover.IsMoving && !_player.isMoveClick)
            {
                _player.ChangeState(FSMState.Idle);
            }
        }
    }
}
