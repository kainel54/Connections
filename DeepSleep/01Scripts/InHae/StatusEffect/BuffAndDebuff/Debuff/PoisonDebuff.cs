using System.Collections;
using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class PoisonDebuff : AilmentStat
{
    private float _delay = 0.5f;
    private bool _buffTime;
    private MonoBehaviour mono;

    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        mono = entity as MonoBehaviour;

        _buffTime = true;
        _statCompo.GetElement("Speed").AddModify("PoisonDebuff", -3, EModifyMode.Add);
    }

    private IEnumerator PoisonCoroutine()
    {
        while (_buffTime)
        {
            _statCompo.GetElement("Health").AddModify("PoisonDebuff", -1, EModifyMode.Add);
            yield return new WaitForSeconds(_delay);
        }
    }

    public override void RemoveStatus()
    {
        _buffTime = false;
        _statCompo.GetElement("Speed").RemoveModify("PoisonDebuff", EModifyMode.Add);

        base.RemoveStatus();
    }
}
