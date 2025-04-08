using UnityEngine;
using YH.Entities;

namespace YH.Enemy
{
    public class WizardEnemyAnimator : EnemyAnimator
    {
        private WizardEnemyAttackCompo _attackCompo;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _attackCompo = entity.GetCompo<WizardEnemyAttackCompo>();
        }

        public void FireEvent() => _attackCompo.FireBall();
    }
}

