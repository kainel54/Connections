using UnityEngine;
using YH.Entities;

namespace YH.Enemy
{
    public class ShamanEnemyAnimator : EnemyAnimator
    {
        private ShamanEnemyAttackCompo _attackCompo;
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _attackCompo = entity.GetCompo<ShamanEnemyAttackCompo>();
        }
        public void CreateEffectEvent() => _attackCompo.CreateEffect();
        public void Sorcery() => _attackCompo.Sorcery();
    }
}

