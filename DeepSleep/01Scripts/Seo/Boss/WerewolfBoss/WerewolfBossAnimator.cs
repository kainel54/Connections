using UnityEngine;
using YH.Entities;

public class WerewolfBossAnimator : EnemyAnimator
{
    private WerewolfBossAttackCompo _attackCompo;


    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);

        _attackCompo = entity.GetCompo<WerewolfBossAttackCompo>();
        _attackCompo.SetBossLevel();
    }


}
