using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class AttackDamageBuff : BuffAndDebuffStat
{
    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        _statCompo.GetElement("AttackPower").AddModify("AttackDamageBuff", +20, EModifyMode.Add);
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
        _statCompo.GetElement("AttackPower").RemoveModify("AttackDamageBuff", EModifyMode.Add);
    }

}
