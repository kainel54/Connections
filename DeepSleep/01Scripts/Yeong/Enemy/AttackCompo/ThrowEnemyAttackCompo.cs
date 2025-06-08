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
        private ThrowEnemy _throwEnemy;

        private void Awake()
        {
            _throwEnemy = _enemy as ThrowEnemy;
        }

        public void ShootingGranade()
        {
            if (_throwEnemy.isDivision)
            {
                for (byte i = 1; i <= 3; i++)
                {
                    Fire(i);
                }
            }
            else
            {
                Fire(0);
            }
           
        }

        private void Fire(byte idx)
        {
            var grenade = PoolManager.Instance.Pop(ProjectileType.Grenade) as Grenade;
            Vector3 targetPosition = _enemy.player.transform.position;
            _targetPos = targetPosition + new Vector3(Random.Range(-idx, idx), 0, Random.Range(-idx, idx));
            grenade.FireGrenade(45, _firePos.position, _targetPos, _enemy);
            BombDisplaySetting(grenade.timeToTarget);
        }

        public void BombDisplaySetting(float timeToTarget)
        {
            var display = PoolManager.Instance.Pop(UIPoolingType.BombCircleDisplay) as BombDisplay;
            Vector3 pos = new Vector3(_targetPos.x, _targetPos.y+0.3f, _targetPos.z);
            display.SettingCircle(2.25f, pos, timeToTarget);
        }
    }
}

