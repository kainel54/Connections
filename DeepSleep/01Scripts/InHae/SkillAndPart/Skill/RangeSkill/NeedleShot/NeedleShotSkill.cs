using System.Collections;
using UnityEngine;

public class NeedleShotSkill : RangeSkill
{
    protected override IEnumerator RangeAttack(Transform hipShootTrm)
    {
        _forwardVector = hipShootTrm.up;
        return base.RangeAttack(hipShootTrm);
    }
}
