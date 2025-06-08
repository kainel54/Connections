using YH.StatSystem;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace YH.Entities
{
    public class EnemyMovement : MonoBehaviour, IEntityComponent, IAfterInitable
    {
        [SerializeField] private StatElementSO moveSpeedSO;

        private Rigidbody _rbCompo;
        private NavMeshAgent _navAgent;
        private BTEnemy _enemy;
        private EntityRenderer _renderer;
        private EntityStat _statCompo;
        private StatElement _speedStat;
        private Collider _collider;

        private Vector3 _nextPathPoint;
        private bool _isAutoRotate;
        private Vector2 _movement;

        public Vector3 Velocity => _rbCompo.linearVelocity;
        public bool IsManualMove { get; private set; }
        public bool IsManualRotation { get; private set; }

        public float SpeedMultiplier { get; set; } = 1f;

        public event Action<bool> DodgeAction;

        public void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;

            _navAgent = entity.GetComponent<NavMeshAgent>();
            _rbCompo = entity.GetComponent<Rigidbody>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _collider = entity.GetComponent<Collider>();
            _statCompo = entity.GetCompo<EntityStat>(true);
        }

        public void AfterInit()
        {
            _speedStat = _statCompo.GetElement(moveSpeedSO);
            _navAgent.speed = _speedStat.Value * SpeedMultiplier;
        }


        private IEnumerator NavAgentDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _navAgent.enabled = true;
        }

        private void Update()
        {
            if (_isAutoRotate)
                _enemy.FaceToTarget(_enemy.player.position);
        }

        public Vector3 GetNextPathPoint()
        {
            NavMeshPath path = _navAgent.path;

            if (path.corners.Length < 2)
            {
                return _navAgent.destination;
            }

            for (int i = 0; i < path.corners.Length; i++)
            {
                float distance = Vector3.Distance(_navAgent.transform.position, path.corners[i]);

                if (distance < 1 && i < path.corners.Length - 1)
                {
                    _nextPathPoint = path.corners[i + 1];
                    return _nextPathPoint;
                }
            }

            return _nextPathPoint;
        }

        public void SetManualMove(bool isManualMove) => IsManualMove = isManualMove;
        public void SetManualRotation(bool isManualRotation) => IsManualRotation = isManualRotation;

        public void SetStop(bool isStop)
        {
            _navAgent.ResetPath();            // 경로 초기화
            _navAgent.velocity = Vector3.zero; // 속도 완전 정지
            _navAgent.isStopped = isStop;
        }
        public void SetSpeed(float speed) => _navAgent.speed = speed;
        public void SetDestination(Vector3 destination) => _navAgent.SetDestination(destination);

        public bool GetIsArrived(float stopOffset)
            => !_navAgent.isPathStale && _navAgent.remainingDistance < _navAgent.stoppingDistance + stopOffset;


        public void SetNavAgentWarp(Vector3 setPosition, Quaternion setRotation)
        {
            _navAgent.Warp(setPosition);
            transform.rotation = setRotation;
            _navAgent.updatePosition = true;
            _navAgent.updateRotation = true;
        }

        public void SetStartingWarpSetting()
        {
            _navAgent.updatePosition = false;
            _navAgent.updateRotation = false;
        }

        public void ForceMove(Vector3 targetPos)
        {
            NavMeshEnable(false);
            transform.position = targetPos;
        }

        public void NavMeshEnable(bool value)
        {
            _navAgent.enabled = value;
        }

        public void Setting(Vector3 spawnPos, Vector3 targetPos)
        {
            _navAgent.Warp(spawnPos); // 강제로 위치 설정
            SetDestination(targetPos);
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            SpeedMultiplier = speedMultiplier;
            _navAgent.speed = _speedStat.Value * SpeedMultiplier;
        }

        public void SetPos(Vector3 pos)
        {
            _enemy.transform.position = pos;
            StartCoroutine(NavAgentDelay());
        }

        public void Dodge()
        {
            if (_enemy.GetCompo<EntityHealth>().GetDie())
                return;
            Vector3? randPos = GetRandomAvailableDirection();
            if (randPos == null)
                return;
            _enemy.transform.position = randPos.Value;
            DodgeAction?.Invoke(true);
            DOVirtual.DelayedCall(0.2f, () => DodgeAction?.Invoke(false));
            SetPos(randPos.Value);
        }

        private Vector3? GetRandomAvailableDirection()
        {
            Vector3 origin = transform.position;

            // 전후좌우 방향 정의
            Vector3[] directions = new Vector3[]
            {
            -transform.right,       // 왼쪽
            transform.right         // 오른쪽
            };

            List<Vector3> availablePositions = new List<Vector3>();

            foreach (var dir in directions)
            {
                if (!Physics.Raycast(origin, dir, 5, _enemy.whatIsWall))
                {
                    // 벽이 없다면 그 방향으로 이동한 위치 저장
                    Vector3 targetPos = origin + dir.normalized * 5;
                    availablePositions.Add(targetPos);
                }
            }

            if (availablePositions.Count > 0)
            {
                int index = Random.Range(0, availablePositions.Count);
                return availablePositions[index];
            }

            return null;
        }

        public void Dispose()
        {

        }
    }
}
