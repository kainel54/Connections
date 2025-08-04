using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpinksBossEnemy : BTEnemy, IBossComponent
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
