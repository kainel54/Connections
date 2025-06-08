using UnityEngine;

public class DashEnemy : DefaultEnemy
{
    public override void OnPop()
    {
        base.OnPop();
    }

    public override void OnDie()
    {
        base.OnDie();
        GetCompo<EnemyDamageCaster>().SetDamageCaster(false);
    }
}
