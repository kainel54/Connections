using System;
using System.Collections.Generic;
using YH.Animators;
using YH.Entities;
using YH.FSM;
using UnityEngine;
using YH.Core;

namespace YH.Players
{
    public class PointNClickPlayer : Player
    {
        public EntityStateListSO playerFSM;

        private StateMachine _stateMachine;

        private EntityAIMover _mover;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(playerFSM, this);
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();

            PlayerInput.DashEvent += HandleDash;
            PlayerInput.MoveEvent += HandleMove;
            _mover = GetCompo<EntityAIMover>();
        }

        private void HandleMove()
        {
            _mover.SetMovement(PlayerInput.GetWorldMousePosition());
        }

        protected override void OnDestroy()
        {
            PlayerInput.ClearSubscription();
        }


        

        private void HandleDash()
        {
            ChangeState(FSMState.Dash);
        }

        private void Start()
        {
            _stateMachine.Initialize(FSMState.Idle);
        }

        public void ChangeState(FSMState stateName) => _stateMachine.ChangeState(stateName);


        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        public void AnimationFinishTrigger() => _stateMachine.currentState.AnimationEndTrigger();

    }
}
