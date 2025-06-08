using System;
using System.Collections;
using DG.Tweening;
using YH.Animators;
using YH.StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace YH.Entities
{
    public class EntityAIMover : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatElementSO moveSpeedSO;

        // Anim Params
        public Vector3 Velocity => _velocity;
        private Vector3 _velocity;

        public bool CanManualMove { get; set; } = true;
        public float SpeedMultiplier { get; set; } = 1f;
        public bool IsMoving { get; private set; }
        public bool IsStopped => _agent.isStopped;
        public NavMeshAgent Agent => _agent;

        private Rigidbody _rb;
        private NavMeshAgent _agent;
        private Entity _entity;
        private EntityRenderer _renderer;
        private EntityStat _statCompo;
        private StatElement _speedStat;

        [HideInInspector] public Vector3 EndPos;
        private Collider _collider;

        [SerializeField] private float _knockBackThreshold = 0.1f;
        [SerializeField] private float _maxKnockBack = 1.0f;

        private float _knockBackStartTime;
        private bool _isKnockBack;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _agent = entity.GetComponent<NavMeshAgent>();
            _rb = entity.GetComponent<Rigidbody>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _collider = entity.GetComponent<Collider>();
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public void AfterInit()
        {
            _speedStat = _statCompo.GetElement(moveSpeedSO);
            _agent.speed = _speedStat.Value;
            _collider.enabled = false;
        }

        public void AddForceToEntity(Vector3 force)
        {
            if (_isKnockBack) return;
            StartCoroutine(ApplyKnockBack(force));
        }

        private IEnumerator ApplyKnockBack(Vector3 force)
        {
            _isKnockBack = true;
            _agent.enabled = false;
            _rb.useGravity = true;
            _rb.isKinematic = false;
            _collider.enabled = true;

            _rb.AddForce(force, ForceMode.Impulse);
            _knockBackStartTime = Time.time;

            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(IsKnockBackEnded);

            // NavMesh 위의 안전 지점으로 Warp
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                Vector3 validPos = hit.position;
                DisableRigidBody();
                _agent.Warp(validPos);
            }
            else
            {
                DisableRigidBody();
                _agent.Warp(transform.position); // fallback
            }

            _isKnockBack = false;
        }

        private bool IsKnockBackEnded()
        {
            return _rb.linearVelocity.magnitude < _knockBackThreshold
                   || Time.time > _knockBackStartTime + _maxKnockBack;
        }

        public void StopImmediately()
        {
            if (_agent.enabled == false) return;
            _agent.isStopped = true;
        }

        public void SetMovement(Vector3 movement)
        {
            if (!CanManualMove) return;

            _agent.isStopped = false;
            EndPos = new Vector3(movement.x, 0, movement.z);
            _agent.SetDestination(movement);
        }

        private void FixedUpdate()
        {
            if (!CanManualMove)
            {
                IsMoving = false;
                _agent.isStopped = true;
                return;
            }

            float targetDistance = Vector3.Distance(EndPos, transform.position);
            IsMoving = targetDistance > 0.1f;
        }

        private void DisableRigidBody()
        {
            _agent.enabled = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _collider.enabled = false;
        }

        public void SetStoppingDistance(float distance)
        {
            _agent.stoppingDistance = distance;
        }
    }
}
