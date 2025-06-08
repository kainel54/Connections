using ObjectPooling;
using System;
using UnityEngine;
using YH.Core;

namespace YH.Enemy
{
    public class ShieldEnemyAttackCompo : EnemyAttackCompo
    {
        public void AttackSetting()
        {
            BombDisplay display = PoolManager.Instance.Pop(UIPoolingType.BombCircleDisplay) as BombDisplay;
            display.SettingCircle(4f, _enemy.transform.position, 1f);
            display.DisplayEndEvent += HandleShake;
        }

        private void HandleShake(BombDisplay display)
        {
            CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
            display.DisplayEndEvent -= HandleShake;
        }
    }
}

