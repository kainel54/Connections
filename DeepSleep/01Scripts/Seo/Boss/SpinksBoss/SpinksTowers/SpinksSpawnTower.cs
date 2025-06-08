using UnityEngine;

public class SpinksSpawnTower : SpinksBossTower
{
    public override void UseSkill()
    {
        base.UseSkill();
        _attackCompo.SpawnEnemy();
    }
}
