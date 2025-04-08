using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class InundationDebuff : BuffAndDebuffStat
{
    private bool _isflooded = false;

    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        _statCompo.GetElement("Speed").AddModify("InundationDebuff", -20, EModifyMode.Percnet);
        _statCompo.GetElement("AttackSpeed").AddModify("InundationDebuff", -30, EModifyMode.Percnet);
        _isflooded = true;
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
        _statCompo.GetElement("Speed").RemoveModify("InundationDebuff", EModifyMode.Percnet);
        _statCompo.GetElement("AttackSpeed").RemoveModify("InundationDebuff", EModifyMode.Percnet);
        _isflooded = false;
    }
}
