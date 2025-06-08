using System.Collections;
using UnityEngine;

public class SpinksFallMagicTower : SpinksBossTower
{
    public override void UseSkill()
    {
        base.UseSkill();

        StartCoroutine(SpawnFallMagic());
    }


    private IEnumerator SpawnFallMagic()
    {
        _attackCompo.FallMagicAttack();
        yield return new WaitForSeconds(0.8F);
        _attackCompo.FallMagicAttack();
        yield return new WaitForSeconds(0.8F);
        _attackCompo.FallMagicAttack();

    }
}
