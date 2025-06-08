using DG.Tweening;
using ObjectPooling;
using System;
using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.EventSystem;

namespace YH.Enemy
{
    public class SelfBombEnemyAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private EnemyDamageCaster _bombCaster;
        [SerializeField] private PoolingItemSO _explosionEffect;
        [SerializeField] private GameEventChannelSO _spawnChannel;
        [SerializeField] private SoundSO _bombSound;
        [SerializeField] private float _displayLifeTime;  

        public event Action<bool> OnBombEvent;

        private BombDisplay _display;
        public float radius { get; set; }

        private void Update()
        {
            if(_display)
                _display.transform.position = _enemy.transform.position;
        }
        public void SelfBombDisplaySetting()
        {
            var display = PoolManager.Instance.Pop(UIPoolingType.BombCircleDisplay) as BombDisplay;
            Vector3 pos = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y + 0.3f, _enemy.transform.position.z);
            _display = display;
            _display.transform.SetParent(_enemy.transform);
            display.SettingCircle(radius, pos, _displayLifeTime);
            display.DisplayEndEvent += HandleBomb;
        }

        private void HandleBomb(BombDisplay display)
        {
            _display = null;

            var evt = SpawnEvents.EffectSpawn;
            evt.position = transform.position;
            evt.rotation = Quaternion.identity;
            evt.effectItem = _explosionEffect;
            evt.scale = new Vector3(radius * 0.25f, radius * 0.25f, radius * 0.25f);
            _spawnChannel.RaiseEvent(evt);
            CameraManager.Instance.ShakeCamera(4, 4, 0.15f);

            SoundPlayer sound = PoolManager.Instance.Pop(ObjectType.SoundPlayer) as SoundPlayer;
            sound.PlaySound(_bombSound);
            sound.transform.position = _enemy.transform.position;

            SelfBombEnemy enemy = _enemy as SelfBombEnemy;
            enemy.SetBombActive(false);

            _bombCaster.OnceCast();
            _enemy.GetCompo<EntityHealth>().Die();
            display.DisplayEndEvent -= HandleBomb;
            OnBombEvent?.Invoke(false);
        }

        public void BigBigPart()
        {
            radius += 0.1f;
            OverlapCircleDamageCaster bombcaster = _bombCaster.GetCast(0) as OverlapCircleDamageCaster;
            bombcaster.damageRadius = radius;
            _bombCaster.SetCast(bombcaster,0);
            if (_display)
            {
                _display.transform.DOScaleX(radius * 2f, 0.05f);
                _display.transform.DOScaleY(radius * 2f, 0.05f);
            }
        }

        public void SetDisplay(BombDisplay display)
        {
            _display.DisplayEndEvent -= HandleBomb;
            _display = display;
        }

        public BombDisplay GetDisplay()
        {
            return _display;
        }

    }
}


