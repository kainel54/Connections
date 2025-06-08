using System.Collections.Generic;
using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class AilmentStat : StatusStat, ILifeTimeStatus
{
    protected EntityStat _statCompo;

    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        _statCompo = entity.GetCompo<EntityStat>();
    }

    public void SetLifeTime(float lifetime)
    {
        lifeMode = true;
        _totalTimer = lifetime;
        lifeTime = lifetime;
    }
}
