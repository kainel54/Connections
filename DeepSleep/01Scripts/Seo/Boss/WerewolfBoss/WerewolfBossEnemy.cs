using UnityEngine;

public class WerewolfBossEnemy : BTEnemy, IBossComponent
{
    private BossLevelRoom _bossLevel;

    public BossLevelRoom GetBossLevel()
    {
        if (_bossLevel == null)
            _bossLevel = FindAnyObjectByType<BossLevelRoom>();
        return _bossLevel;
    }

    public override void Setinvincible(bool enableValue)
    {
        _collider.enabled = !enableValue;
    }
}
