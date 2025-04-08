using UnityEngine;
using YH.Animators;
using YH.Enemy;
using YH.Entities;
using YH.StatSystem;

public class ThrowEnemyAnimator : EnemyAnimator
{
    private ThrowEnemyAttackCompo _attackCompo;
    
    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        _attackCompo = entity.GetCompo<ThrowEnemyAttackCompo>();
    }
    public void ThrowAttack() => _attackCompo.ShootingGranade();
    
}
