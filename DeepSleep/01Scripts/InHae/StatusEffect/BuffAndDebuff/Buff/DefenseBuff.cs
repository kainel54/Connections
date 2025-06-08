using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class DefenseBuff : BuffAndDebuffStat
{
    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        _statCompo.GetElement("Defense").AddModify("DefenseBuff", +20, EModifyMode.Percnet);
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
        _statCompo.GetElement("Defense").RemoveModify("DefenseBuff", EModifyMode.Percnet);
    }
}
