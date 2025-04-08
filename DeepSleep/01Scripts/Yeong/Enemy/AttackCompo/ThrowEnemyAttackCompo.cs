using ObjectPooling;
using UnityEngine;
using YH.EventSystem;

namespace YH.Enemy
{
    public class ThrowEnemyAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private GameEventChannelSO _spawnChannel;
        [SerializeField] private Transform _firePos;
        [SerializeField] private float _displayLifeTime;

        private Vector3 _targetPos;


        public void ShootingGranade()
        {
            var grenade = PoolManager.Instance.Pop(PoolingType.Grenade) as Grenade;
            Vector3 targetPosition = _enemy.player.transform.position;
            _targetPos = targetPosition + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            grenade.FireGrenade(45, _firePos.position, _targetPos, _enemy);
            BombDisplaySetting(grenade.timeToTarget);
        }

        public void BombDisplaySetting(float timeToTarget)
        {
            var display = PoolManager.Instance.Pop(PoolingType.BombCircleDisplay) as BombDisplay;
            Vector3 pos = new Vector3(_targetPos.x, _targetPos.y+0.3f, _targetPos.z);
            display.SettingCircle(2.25f, pos, timeToTarget);
        }
    }
}

