using System;
using System.Collections;
using DG.Tweening;
using YH.Animators;
using YH.StatSystem;
using UnityEngine;
using UnityEngine.AI;
using YH.Players;

namespace YH.Entities
{
    public class EntityAIMover : MonoBehaviour, IEntityComponent, IAfterInitable
    {
        [SerializeField] private StatElementSO moveSpeedSO;

        // Anim Params
        [SerializeField] private AnimParamSO _moveSpeed;
        public Vector3 Velocity => _velocity;
        private Vector3 _velocity;

        public bool CanManualMove { get; set; } = true;
        public bool CanManualRotate { get; set; } = true;
        public float SpeedMultiplier { get; set; } = 1f;
        public bool IsMoving;
        public bool IsStopped => _navAgent.isStopped;
        public NavMeshAgent Agent => _navAgent;

        private Rigidbody _rb;
        private NavMeshAgent _navAgent;
        private Entity _entity;
        private MOBAPlayer _player;
        private EntityRenderer _renderer;
        private EntityStat _statCompo;
        private StatElement _speedStat;

        [HideInInspector] public Vector3 EndPos;
        private Collider _collider;

        [SerializeField] private float _knockBackThreshold = 0.1f;
        [SerializeField] private float _maxKnockBack = 1.0f;
        [SerializeField] private AnimationCurve _verticalCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _forwardCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private LayerMask _whatIsGround;
        
        private float _knockBackStartTime;
        private bool _isKnockBack;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _player = entity as MOBAPlayer;
            
            _navAgent = entity.GetComponent<NavMeshAgent>();
            _rb = entity.GetComponent<Rigidbody>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _collider = entity.GetComponent<Collider>();
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public void AfterInit()
        {
            _speedStat = _statCompo.GetElement(moveSpeedSO);
            _navAgent.speed = _speedStat.Value;
            
            if (_speedStat != null)
                _speedStat.OnValueChanged += HandleSpeedChangedEvent;
            
            _navAgent.autoBraking = false;
            _navAgent.stoppingDistance = 0f;

            _navAgent.acceleration = 999f;
            _navAgent.angularSpeed = 5000f;  

            _navAgent.updateRotation = false;
            EndPos = _entity.transform.position;
        }

        public void Dispose()
        {
            
        }

        private void OnDestroy()
        {
            if (_speedStat != null)
                _speedStat.OnValueChanged -= HandleSpeedChangedEvent;
        }

        private void HandleSpeedChangedEvent(float arg1, float arg2)
        {
            _navAgent.speed = _speedStat.Value;
        }

        public void AddForceToEntity(Vector3 force)
        {
            if (_isKnockBack) return;
            StartCoroutine(ApplyKnockBack(force));
        }

        private IEnumerator ApplyKnockBack(Vector3 force)
        {
            _isKnockBack = true;
            _navAgent.enabled = false;
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
                _navAgent.Warp(validPos);
            }
            else
            {
                DisableRigidBody();
                _navAgent.Warp(transform.position); // fallback
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
            if (_navAgent.enabled == false) return;
            _navAgent.isStopped = true;
            _navAgent.ResetPath();
        }

        public void SetMovement(Vector3 movement)
        {
            if (!CanManualMove) 
                return;

            Vector3 currentVel = _navAgent.velocity;
            
            Vector3 movementNoYPos = movement;
            movementNoYPos.y = 0f;
            Vector3 selfPos = transform.position;
            selfPos.y = 0f;
            
            Vector3 targetDir = (movementNoYPos - selfPos).normalized;

            if (currentVel.sqrMagnitude < 0.01f)
            {
                InstantRotate(targetDir);
                EndPos = movement;
                _navAgent.isStopped = false;
                _navAgent.SetDestination(movement);
                return;
            }

            Vector3 velDir = currentVel.normalized;
            float dot = Vector3.Dot(velDir, targetDir);

            if (dot < 0f)
            {
                _navAgent.isStopped = true;
                _navAgent.velocity = Vector3.zero;
                _navAgent.ResetPath();

                InstantRotate(targetDir);

                EndPos = movement;
                _navAgent.SetDestination(movement);
                _navAgent.isStopped = false;
            }
            else
            {
                EndPos = movement;
                _navAgent.isStopped = false;
                _navAgent.SetDestination(movement);
            }
            IsMoving = true;
        }

        private void FixedUpdate()
        {
            if (!CanManualMove)
            {
                IsMoving = false;

                if(_navAgent.enabled) _navAgent.isStopped = true;
                return;
            }

            if (CanManualRotate)
                ApplyRotate();

            if (_navAgent.enabled)
            {
                IsMoving = !_navAgent.isStopped &&
                !_navAgent.pathPending &&
                _navAgent.remainingDistance > _navAgent.stoppingDistance + 0.05f;
            }
            
            Vector3 movementNoYPos = _navAgent.destination;
            movementNoYPos.y = 0f;
            Vector3 selfPos = transform.position;
            selfPos.y = 0f;

            float animSpeed = Vector3.Distance(selfPos, movementNoYPos);
            float inverseLerp = Mathf.InverseLerp(0f, 1f, animSpeed);
            float lerp = Mathf.Lerp(0.3f, 1f, inverseLerp);
            lerp = (float)Math.Round(lerp, 1); 
            _renderer.SetParam(_moveSpeed, lerp);
        }

        private void ApplyRotate()
        {
            Vector3 steerTarget = _navAgent.steeringTarget;
            Vector3 dir = steerTarget - _entity.transform.position;
            dir.y = 0;

            if (dir.sqrMagnitude < 0.0001f) return;

            Quaternion lookRot = Quaternion.LookRotation(dir);
            _entity.transform.rotation = lookRot;
        }

        private void InstantRotate(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.0001f) return;
            Quaternion lookRot = Quaternion.LookRotation(direction);
            _entity.transform.rotation = lookRot;
        }
            
        private void DisableRigidBody()
        {
            _navAgent.enabled = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }

        public void SetStoppingDistance(float distance)
        {
            _navAgent.stoppingDistance = distance;
        }

        public void MoveForward(float forwardDistance, float duration)
        {
            StartCoroutine(MoveForwardCoroutine(forwardDistance, duration));
        }

        public void ApplyVerticalOffset(float verticalHeight, float duration)
        {
            StartCoroutine(VerticalOffsetCoroutine(verticalHeight, duration));
        }

        private IEnumerator MoveForwardCoroutine(float forwardDistance, float duration)
        {
            _navAgent.enabled = false;
            _rb.useGravity = true;
            _rb.isKinematic = false;

            Vector3 forwardDir = _entity.transform.forward.normalized;
            Vector3 start = _entity.transform.position;

            
            float adjustedDistance = forwardDistance;
            RaycastHit hit;
            if (Physics.Raycast(start, forwardDir, out hit, forwardDistance, _player.WhatisWall))
            {
                adjustedDistance = 0;
            }

            Vector3 end = start + forwardDir * adjustedDistance;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                float curveZ = _forwardCurve.Evaluate(t);
                Vector3 newPos = Vector3.Lerp(start, end, curveZ);

                // 현재 Y 위치 유지
                newPos.y = _rb.position.y;

                _rb.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
            }

            SnapToNavMesh(_rb.position);
        }

        private IEnumerator VerticalOffsetCoroutine(float verticalHeight, float duration)
        {
            _navAgent.enabled = false;
            _rb.useGravity = true;
            _rb.isKinematic = false;

            float startY = _rb.position.y;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float curveY = _verticalCurve.Evaluate(t);

                Vector3 currentPos = _rb.position;
                currentPos.y = startY + curveY * verticalHeight;

                _rb.MovePosition(currentPos);
                yield return new WaitForFixedUpdate();
            }

            SnapToNavMesh(_rb.position);
        }

        private void SnapToNavMesh(Vector3 currentPosition)
        {
            if (NavMesh.SamplePosition(currentPosition, out var hit, 2f, NavMesh.AllAreas))
            {
                FinishMove(hit.position, currentPosition.y);
            }
            else
            {
                FinishMove(currentPosition, currentPosition.y);
            }
        }

        private void FinishMove(Vector3 targetPos, float baseOffsetY)
        {
            if (Physics.Raycast(_entity.transform.position, Vector3.down, out var groundHit, 0.3f, _whatIsGround))
            {
                DisableRigidBody();
                _navAgent.Warp(targetPos);
                _navAgent.enabled = true;
            }
            else
            {
                DOVirtual.DelayedCall(0.05f, () =>
                {
                    DisableRigidBody();
                    if (!_navAgent.Warp(targetPos))
                        _navAgent.Warp(_player.CurrentSpawnPos);
                    _navAgent.enabled = true;
                });
            }
        }


        public void RotateToDirection(Vector3 direction)
        {
            direction.y = 0f; // 수평 회전만 적용
            if (direction.sqrMagnitude < 0.0001f)
                return;

            Quaternion targetRot = Quaternion.LookRotation(direction.normalized);
            _entity.transform.rotation = targetRot;
        }
    }
}
