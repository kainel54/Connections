using ObjectPooling;
using System;
using UnityEngine;
using YH.Projectile;

public class SpinksFallMagic : ProjectileObj
{

    public void SetLifeTime(Vector3 point, float lifeTime)
    {
        var display = PoolManager.Instance.Pop(UIPoolingType.BombCircleDisplay) as BombDisplay;
        display.SettingCircle(2.5f, point + Vector3.up * 0.3f, lifeTime);
    }

}
