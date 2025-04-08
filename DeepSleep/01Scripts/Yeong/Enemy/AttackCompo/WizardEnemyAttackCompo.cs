using ObjectPooling;
using UnityEngine;
using YH.EventSystem;
using YH.StatSystem;

namespace YH.Enemy
{
    public class WizardEnemyAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private GameEventChannelSO _spawnChannel;
        [SerializeField] private Transform _firePos;
        [SerializeField] private StatElementSO _damageSO;
        [SerializeField] private float _bulletSpeed, _shootingRange, _impactForce;
        private BulletPayload _bulletPayload;

        private void Awake()
        {
            _bulletPayload = new BulletPayload();
        }
        public void FireBall()
        {
            EnemyFireBall fireBall = PoolManager.Instance.Pop(PoolingType.EnemyFireBall) as EnemyFireBall;
            Vector3 targetDirection = _enemy.transform.forward;//(_enemy.player.transform.position - _firePos.position).normalized;
            Quaternion targetDir = Quaternion.LookRotation(targetDirection);
            SetPayload(_enemy.transform.forward, _bulletSpeed);
            fireBall.Fire(_firePos.position, targetDir, _bulletPayload, _enemy);
        }


        private void SetPayload(Vector3 bulletDirection, float bulletSpeed)
        {
            _bulletPayload.mass = 20f / bulletSpeed;
            _bulletPayload.shootingRange = _shootingRange;
            _bulletPayload.impactForce = _impactForce;
            _bulletPayload.damage = _enemy.GetCompo<StatCompo>().GetElement(_damageSO).Value;
            _bulletPayload.velocity
                = bulletDirection * bulletSpeed;
        }
    }
}

