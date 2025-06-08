using UnityEngine;
using YH.Projectile;

public class SandTornado : ProjectileObj
{
    protected override void OnCollisionEnter(Collision other)
    {
        ApplyDamageToTarget(other);
        ApplyKnockBackToTarget(other);
        PoolManager.Instance.Push(this);
    }
}
