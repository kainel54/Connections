using System.Collections;
using UnityEngine;
using YH.Entities;
using YH.StatSystem;

public class BurnDebuff : BuffAndDebuffStat
{
    private float _delay = 0.5f;
    private bool _buffTime;
    private MonoBehaviour mono;

    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        mono = entity as MonoBehaviour;

        _buffTime = true;
    }

    private IEnumerator BurnCoroutine()
    {
        while (_buffTime)
        {
            _statCompo.GetElement("Health").AddModify("BurnDebuff", -1, EModifyMode.Add);
            yield return new WaitForSeconds(_delay);
        }
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
    }
}
