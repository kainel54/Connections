using UnityEngine;
using YH.Boss;
using YH.Entities;

public class SkulBossAnimator : EnemyAnimator
{
    private SkulBossAttackCompo _attackCompo;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        _attackCompo = entity.GetCompo<SkulBossAttackCompo>();
    }
    public void FireSphere() => _attackCompo.FireSphere();
    public void DashAttack() => _attackCompo.DashAttack();

}
