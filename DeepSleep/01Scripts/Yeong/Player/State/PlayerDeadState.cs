using DG.Tweening;
using System.Collections;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Players
{
    public class PlayerDeadState : EntityState
    {
        private MOBAPlayer _player;
        private EntityAIMover _mover;

        public PlayerDeadState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as MOBAPlayer;
            _mover = _player.GetCompo<EntityAIMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
            DOVirtual.DelayedCall(2.1f, () =>
            {
                _player.OnDieEvent.Invoke();
            });
        }
    }
}

