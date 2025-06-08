using System;
using DG.Tweening;
using YH.Animators;
using YH.StatSystem;
using UnityEngine;
using YH.Players;

namespace YH.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent, IAfterInitable
    {
        [SerializeField] private StatElementSO moveSpeedSO;
        [field:SerializeField] public float rotationSpeed;
        private Vector3 _movement;
        [SerializeField] private float _gravity = -9.8f;

        private Vector3 _velocity;
        private float _verticalVelocity;
        public bool IsGround => _characterCompo.isGrounded;

        [Header("AnimParams")]
        public bool CanManualMove { get; set; } = true; //키보드로 움직임 가능
        public float SpeedMultiplier { get; set; } = 1f;

        //private Rigidbody _rbCompo;
        private CharacterController _characterCompo;
        private Entity _entity;
        private EntityRenderer _renderer;
        private EntityStat _statCompo;
        private StatElement _speedStat;
        public Quaternion targetRotation { get; set; }

        //private Collider _collider;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _characterCompo = entity.GetComponent<CharacterController>();
            //_rbCompo = entity.GetComponent<Rigidbody>();
            _renderer = entity.GetCompo<EntityRenderer>();
            //_collider = entity.GetComponent<Collider>();
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public void AfterInit()
        {
            _speedStat = _statCompo.GetElement(moveSpeedSO);
        }
        protected void FixedUpdate()
        {
            //중력을 적용하는 작업
            ApplyGravity();
            ApplyRotation();
            // 그걸 기반으로 움직이는 작업
            Move();
        }

        private void ApplyRotation()
        {
            _entity.transform.rotation = Quaternion.Lerp(_entity.transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }

        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity <= 0)
            {
                _verticalVelocity = -0.5f;
            }
            else
            {
                _verticalVelocity += _gravity * Time.fixedDeltaTime;
            }
            _velocity.y = _verticalVelocity;
        }


        private void Move()
        {
            if(_characterCompo.enabled)
            _characterCompo.Move(_velocity);
        }

        public void SetMovement(Vector3 movement, bool isRotation = true)
        {
            if (!CanManualMove) return;
            _velocity = movement * 10 * Time.fixedDeltaTime;
            if (_velocity.sqrMagnitude > 0 && isRotation)
            {
                targetRotation = Quaternion.LookRotation(_velocity);
            }
        }

        public void StopImmediately()
        {
            _velocity = Vector3.zero;
        }


        public void GetKnockBack(Vector3 force)
        {
            //_entity.GetCompo<HealthCompo>.actionData.knockBackPower = force.magnitude;

            //_targetRotation = Quaternion.LookRotation(-force.normalized);
            //_agent.transform.rotation = _targetRotation;

            //Player player = _agent as Player;
            //player.StateMachine.ChangeState(PlayerStateEnum.Hurt);
        }

        public void Dispose()
        {

        }
    }
}
