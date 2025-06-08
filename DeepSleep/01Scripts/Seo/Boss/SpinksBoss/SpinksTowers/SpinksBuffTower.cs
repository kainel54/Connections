using UnityEngine;

public class SpinksBuffTower : SpinksBossTower
{

    public override void UseSkill()
    {
        base.UseSkill();
        _attackCompo.BuffGive();
    }
}
