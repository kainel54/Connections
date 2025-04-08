using YH.StatSystem;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.XR.OpenVR;
using YH.Players;

namespace YH.Entities
{
    public class EnemyMovement : MonoBehaviour, IEntityComponent, IAfterInitable
    {
        [SerializeField] private StatElementSO moveSpeedSO;

        private Rigidbody _rbCompo;
        private NavMeshAgent _navAgent;
        private BTEnemy _enemy;
        private EntityRenderer _renderer;
        private StatCompo _statCompo;
        private StatElement _speedStat;
        private Collider _collider;

        private Vector3 _nextPathPoint;
        private bool _isAutoRotate;
        private Vector2 _movement;

        public Vector3 Velocity => _rbCompo.linearVelocity;
        public bool IsManualMove { get; private set; }
        public bool IsManualRotation { get; private set; }

        public float SpeedMultiplier { get; set; } = 1f;

        public void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;

            _navAgent = entity.GetComponent<NavMeshAgent>();
            _rbCompo = entity.GetComponent<Rigidbody>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _collider = entity.GetComponent<Collider>();
            _statCompo = entity.GetCompo<StatCompo>(true);
        }

        public void AfterInit()
        {
            _speedStat = _statCompo.GetElement(moveSpeedSO);
            _navAgent.speed = _speedStat.Value*SpeedMultiplier;
        }

        public void AddForceToEntity(Vector3 force, ForceMode mode = ForceMode.Impulse)
        {
            _navAgent.enabled = false;
            _rbCompo.AddForce(force, mode);
            StartCoroutine(NavAgentDelay());
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

        public void SetStop(bool isStop) => _navAgent.isStopped = isStop;
        public void SetSpeed(float speed) => _navAgent.speed = speed;
        public void SetDestination(Vector3 destination) => _navAgent.SetDestination(destination);

        public bool GetIsArrived(float stopOffset)
            => !_navAgent.isPathStale && _navAgent.remainingDistance < _navAgent.stoppingDistance + stopOffset;

        public void Setting(Vector3 spawnPos,Vector3 targetPos)
        {
            _navAgent.Warp(spawnPos); // 강제로 위치 설정
            SetDestination(targetPos);
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            SpeedMultiplier = speedMultiplier;
            _navAgent.speed = _speedStat.Value * SpeedMultiplier;
        }
    }
}
