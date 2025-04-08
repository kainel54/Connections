using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class SpeedDownDebuff : BuffAndDebuffStat
{
    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        _statCompo.GetElement("Speed").AddModify("SpeedDownDebuff", -35, EModifyMode.Percnet);
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
        _statCompo.GetElement("Speed").RemoveModify("SpeedDownDebuff", EModifyMode.Percnet);
    }
}