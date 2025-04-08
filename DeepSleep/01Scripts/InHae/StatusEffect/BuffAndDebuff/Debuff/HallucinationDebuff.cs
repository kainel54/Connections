using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class HallucinationDebuff : BuffAndDebuffStat
{
    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);

        _statCompo.GetElement("Speed").AddModify("HallucinationDebuff", -20, EModifyMode.Percnet);
        _statCompo.GetElement("AttackSpeed").AddModify("HallucinationDebuff", -30, EModifyMode.Percnet);
    }

    public override void RemoveStatus()
    {
        _statCompo.GetElement("Speed").RemoveModify("HallucinationDebuff", EModifyMode.Percnet);
        _statCompo.GetElement("AttackSpeed").RemoveModify("HallucinationDebuff", EModifyMode.Percnet);

        base.RemoveStatus();
    }
}
