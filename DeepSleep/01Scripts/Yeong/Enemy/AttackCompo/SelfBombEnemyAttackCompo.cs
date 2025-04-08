using ObjectPooling;
using System;
using UnityEngine;
using YH.Core;
using YH.EventSystem;

namespace YH.Enemy
{
    public class SelfBombEnemyAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private EnemyDamageCaster _bombCaster;
        [SerializeField] private PoolingItemSO _explosionEffect;
        [SerializeField] private GameEventChannelSO _spawnChannel;
        [SerializeField] private float _displayLifeTime;

        public event Action<bool> OnBombEvent;

        private Transform _display;

        private void Update()
        {
            if(_display != null )
                _display.position = _enemy.transform.position;
        }
        public void SelfBombDisplaySetting()
        {
            var display = PoolManager.Instance.Pop(PoolingType.BombCircleDisplay) as BombDisplay;
            Vector3 pos = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y + 0.3f, _enemy.transform.position.z);
            display.SettingCircle(4.5f, pos, _displayLifeTime);
            _display = display.transform;
            display.DisplayEndEvent += HandleBomb;
        }

        private void HandleBomb(BombDisplay display)
        {
            var evt = SpawnEvents.EffectSpawn;
            evt.position = transform.position;
            evt.rotation = Quaternion.identity;
            evt.effectItem = _explosionEffect;
            _spawnChannel.RaiseEvent(evt);
            CameraManager.Instance.ShakeCamera(4, 4, 0.15f);

            _bombCaster.OnceCast();
            _enemy.GetCompo<HealthCompo>().Die();
            display.DisplayEndEvent -= HandleBomb;
            OnBombEvent?.Invoke(false);
        }

    }
}


