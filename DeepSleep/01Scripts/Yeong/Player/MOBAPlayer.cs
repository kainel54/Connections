using System;
using YH.Entities;
using YH.FSM;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace YH.Players
{
    public class MOBAPlayer : Player
    {
        public EntityStateListSO playerFSM;

        private StateMachine _stateMachine;
        private EntityAIMover _mover;

        public event Action<float, float, int> DashCoolEvent;
        public Action<int> AttackComboEvent;

        [HideInInspector] public int _maxDashCount = 2;
        [HideInInspector] public int currentDashCount = 2;
        [HideInInspector] public bool isDashing = false;
        private float _dashCooldown = 2f;
        private float _dashCooldownTimer = 0f;

        [field: SerializeField] public List<float> moveForwardTime;
        [field: SerializeField] public List<float> moveYTime;
        [field: SerializeField] public LayerMask WhatisWall;
        public Vector3 CurrentSpawnPos { get; private set; }
        public bool isMoveClick { get; set; }
        public bool isAttackClick;
        public bool IsPlaySkill { get; set; }
        public bool IsDashEnable { get; set; } = true;

        public Action AttackEvent;
        public Action StopEvent;
        public Action MoveEvent;
        public Action DashEvent;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(playerFSM, this);
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();


            _mover = GetCompo<EntityAIMover>();
            PlayerInput.AttackEvent += HandleAttackTrigger;
        }

        private void HandleAttackTrigger(bool isClick)
        {
            if(IsPlaySkill)
                return;
            
            isAttackClick = isClick;
        }

        private void Start()
        {
            _stateMachine.Initialize(FSMState.Idle);
        }

        protected override void Update()
        {
            base.Update();
            _stateMachine.UpdateStateMachine();
            UpdateDashCooldown();
        }

        public override void PlayerLevelMove(Vector3 position)
        {
            _mover.Agent.enabled = false;
            
            transform.position = position;
            CurrentSpawnPos = position;
            
            _mover.Agent.Warp(position);
            _mover.Agent.enabled = true;
        }


        

        public void ChangeState(FSMState stateName)
        {
            _stateMachine.ChangeState(stateName);
        }

        public void AnimationFinishTrigger()
        {
            _stateMachine.currentState.AnimationEndTrigger();
        }

        public void SetDashing(bool isDashing)
        {
            this.isDashing = isDashing;
        }

        private void UpdateDashCooldown()
        {
            if (currentDashCount < _maxDashCount)
            {
                _dashCooldownTimer += Time.deltaTime;

                if (_dashCooldownTimer >= _dashCooldown)
                {
                    _dashCooldownTimer -= _dashCooldown;
                    currentDashCount++;

                    if (currentDashCount >= _maxDashCount)
                    {
                        currentDashCount = _maxDashCount;
                        _dashCooldownTimer = 0f;
                    }
                }

                DashCoolEvent?.Invoke(_dashCooldownTimer, _dashCooldown, currentDashCount);
            }
        }

        public override void SetDead()
        {
            base.SetDead();
            ChangeState(FSMState.Dead);
        }

        protected override void OnDestroy()
        {
            PlayerInput.ClearSubscription();
        }
    }
}
