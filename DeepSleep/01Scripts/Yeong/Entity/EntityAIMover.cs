using System;
using DG.Tweening;
using YH.Animators;
using YH.StatSystem;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace YH.Entities
{
    public class EntityAIMover : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatElementSO moveSpeedSO;
        [Header("AnimParams")]

        public Vector3 Velocity => _velocity;
        private Vector3 _velocity;
        public bool CanManualMove { get; set; } = true;
        public float SpeedMultiplier { get; set; } = 1f;
        public bool IsMoving;
        public bool IsStop => _navAgent.isStopped;
        public NavMeshAgent NavMeshAgent => _navAgent;
        
        private Rigidbody _rbCompo;
        private NavMeshAgent _navAgent;
        private Entity _entity;
        private EntityRenderer _renderer;
        private StatCompo _statCompo;
        private StatElement _speedStat;
        [HideInInspector]public Vector3 EndPos;

        private Collider _collider;

        [SerializeField] private float _knockBackThreshold;
        [SerializeField] private float _maxKnockBack;

        private float _knockBackTime;//현재 넉백시간을 저장
        private bool _isKnockBack;//현재 넉백중인지를 저장

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _navAgent = entity.GetComponent<NavMeshAgent>();
            _rbCompo = entity.GetComponent<Rigidbody>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _collider = entity.GetComponent<Collider>();
            _statCompo = entity.GetCompo<StatCompo>();
        }

        public void AfterInit()
        {
            _speedStat = _statCompo.GetElement(moveSpeedSO);
            _navAgent.speed = _speedStat.Value;
            _collider.enabled = false;
        }

        public void AddForceToEntity(Vector3 force)
        {
            StartCoroutine(ApplyKnockBack(force));
        }

        private IEnumerator ApplyKnockBack(Vector3 force)
        {
            _navAgent.enabled = false;//네브메시가 자꾸 제자리로 가려고 함
            _rbCompo.useGravity = true;
            _rbCompo.isKinematic = false;
            _collider.enabled = true;
            _rbCompo.AddForce(force, ForceMode.Impulse);
            _knockBackTime = Time.time;//넉백 시작타임을 기록하고

            if (_isKnockBack)
            {
                yield break;//코루틴 종료
            }

            _isKnockBack = true;
            yield return new WaitForFixedUpdate();//물리 프레임 만큼 대기
            yield return new WaitUntil(CheckKnockBackEnd);

            DisableRigidBody();

            _navAgent.Warp(transform.position);
            _isKnockBack = false;

            //yield return new WaitForFixedUpdate();
        }
        private bool CheckKnockBackEnd()
        {
            return _rbCompo.linearVelocity.magnitude < _knockBackThreshold || Time.time > _knockBackTime + _maxKnockBack;
        }


        public void StopImmediately()
        {
            if (_navAgent.enabled == false) return;
            _navAgent.isStopped = true;
        }

        public void SetMovement(Vector3 movement) 
        {
            _navAgent.isStopped = false;
            EndPos = new Vector3(movement.x,0,movement.z);
            _navAgent.SetDestination(movement);
        }

        private void FixedUpdate()
        {
            if (!CanManualMove)
            {
                IsMoving = false;
            }
            else
            {
                float targetDistance = Vector3.Distance(EndPos, _entity.transform.position);
                IsMoving = targetDistance > 0.1f ? true : false;
            }
        }

        private void DisableRigidBody()
        {
            _navAgent.enabled = true;
            _rbCompo.linearVelocity = Vector3.zero;
            _rbCompo.angularVelocity = Vector3.zero;
            _rbCompo.useGravity = false;
            _rbCompo.isKinematic = true;
            _collider.enabled = false;
        }

        public void SetStoppingDistance(float distance)
        {
            _navAgent.stoppingDistance = distance;
        }

        
    }
}
