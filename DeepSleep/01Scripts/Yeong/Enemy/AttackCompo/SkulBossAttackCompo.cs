using System.Collections;
using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Enemy;
using YH.EventSystem;
using YH.StatSystem;

namespace YH.Boss
{
    public class SkulBossAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private EnemyDamageCaster _damageCaster;
        [SerializeField] private GameEventChannelSO _spawnChannel;
        [SerializeField] private StatElementSO _damageSO;
        [SerializeField] private float _bulletSpeed, _shootingRange, _impactForce,_fireDelay;

        private OverlapCircleDamageCaster _currentCircleCaster;
        private BulletPayload _bulletPayload;

        [SerializeField] private int _defaultShootCount = 5, _addShootCount = 4;

        private int[] _angleList = { -30, 0, 30 };
        
        private void Awake()
        {
            _bulletPayload = new BulletPayload();
        }

        
        private IEnumerator PatternDelay(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            CameraManager.Instance.ShakeCamera(8, 8, 0.15f);
            _damageCaster.Cast(_currentCircleCaster);
        }


        private void SetPayload(Vector3 bulletDirection,float bulletSpeed)
        {
            _bulletPayload.mass = 20f / bulletSpeed;
            _bulletPayload.shootingRange = _shootingRange;
            _bulletPayload.impactForce = _impactForce;
            _bulletPayload.damage = _enemy.GetCompo<EntityStat>().GetElement(_damageSO).Value;
            _bulletPayload.velocity
                = bulletDirection * bulletSpeed;
        }

        public void FireSphere()
        {
            StartCoroutine(FireSphereRoutine());
        }

        private IEnumerator FireSphereRoutine()
        {
            for (int i = 0; i < _addShootCount; i++)
            {
                float angleStep = 360f / (_defaultShootCount + i);
                for (int j = 0; j < _defaultShootCount + i; j++)
                {
                    var ball = SpawnEvents.BallCreate;
                    ball.position = _enemy.transform.position + new Vector3(0, 1f, 0);
                    ball.owner = _enemy;
                    float angle = j * angleStep;
                    ball.rotation = Quaternion.Euler(0, angle, 0);
                    SetPayload(ball.rotation * _enemy.transform.forward,_bulletSpeed);
                    ball.payload = _bulletPayload;
                    _spawnChannel.RaiseEvent(ball);
                }
                yield return new WaitForSeconds(_fireDelay);
            }
        }


        public void DashAttack()
        {
            _currentCircleCaster = _damageCaster.GetCast(0) as OverlapCircleDamageCaster;
            _damageCaster.Cast(_currentCircleCaster);
        }

    }
}

