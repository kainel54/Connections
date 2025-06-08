using UnityEngine;
using YH.Entities;

namespace YH.Enemy
{
    public class ShieldEnemyAnimator : EnemyAnimator
    {
        private ShieldEnemyAttackCompo _attackCompo;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _attackCompo = entity.GetCompo<ShieldEnemyAttackCompo>();
        }


        public void AttackSetting() => _attackCompo.AttackSetting();
    }

}
