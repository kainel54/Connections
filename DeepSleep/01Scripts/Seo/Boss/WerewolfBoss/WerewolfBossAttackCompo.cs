using UnityEngine;
using YH.Enemy;

public class WerewolfBossAttackCompo : EnemyAttackCompo
{
    public void SetBossLevel()
    {
        IBossComponent bossComponent = _enemy as IBossComponent;
        if (bossComponent != null)
        {
            BossLevelRoom bossLevel = bossComponent.GetBossLevel();
        }
    }

    public BossLevelRoom GetEnemyBossLevel()
    {
        IBossComponent bossComponent = _enemy as IBossComponent;
        if (bossComponent != null)
        {
            BossLevelRoom bossLevel = bossComponent.GetBossLevel();
            return bossLevel;
        }
        return null;
    }
}
