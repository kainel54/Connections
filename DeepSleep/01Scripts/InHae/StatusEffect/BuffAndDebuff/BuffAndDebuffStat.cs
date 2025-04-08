using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class BuffAndDebuffStat : StatusStat, ILifeTimeStatus
{
    protected StatCompo _statCompo;

    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        _statCompo = entity.GetCompo<StatCompo>();
    }

    public void SetLifeTime(float lifetime)
    {
        lifeMode = true;
        _totalTimer = lifetime;
        lifeTime = lifetime;
    }
}
