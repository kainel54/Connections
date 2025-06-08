using DG.Tweening;
using ObjectPooling;
using System;
using UnityEngine;
using YH.Core;
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
        public bool isAttack {  get; private set; }

        private void Awake()
        {
            _bulletPayload = new BulletPayload();
        }
        public void FireBall()
        {
            EnemyFireBall fireBall = PoolManager.Instance.Pop(ProjectileType.EnemyFireBall) as EnemyFireBall;
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
            _bulletPayload.damage = _enemy.GetCompo<EntityStat>().GetElement(_damageSO).Value;
            _bulletPayload.velocity
                = bulletDirection * bulletSpeed;
        }

        public void Beam()
        {
            LightningBeam beam = PoolManager.Instance.Pop(ProjectileType.LightningBeam) as LightningBeam;
            Vector3 targetDirection = _enemy.transform.forward;
            Quaternion targetDir = Quaternion.LookRotation(targetDirection);
            CameraManager.Instance.ShakeCamera(4, 4, 0.3f);
            beam.transform.SetParent(_enemy.transform);
            beam.Setting(_enemy);
            beam.PlayEffect(_firePos.position, targetDir, new Vector3(0.2f, 0.2f, 0.2f));
            isAttack = true;
            DOVirtual.DelayedCall(2f, ()=>{
                isAttack = false;
            });
        }

        public void BeamSetting()
        {
            BombDisplay display = PoolManager.Instance.Pop(UIPoolingType.BombBoxDisplay) as BombDisplay;
            Quaternion forwardRotation = Quaternion.LookRotation(_enemy.transform.forward);
            display.SettingBox(1.5f,15f,_enemy.transform.position, forwardRotation * Quaternion.Euler(-90,180,0), 1.5f);
            display.transform.SetParent(_enemy.transform);
        }
    }
}

