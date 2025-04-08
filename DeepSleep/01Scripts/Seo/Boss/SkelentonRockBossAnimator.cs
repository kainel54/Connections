using UnityEngine;
using YH.Entities;

public class SkelentonRockBossAnimator : EnemyAnimator
{
    private SkelentonRockBossAttackCompo _attackCompo;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        _attackCompo = entity.GetCompo<SkelentonRockBossAttackCompo>();
    }


}
