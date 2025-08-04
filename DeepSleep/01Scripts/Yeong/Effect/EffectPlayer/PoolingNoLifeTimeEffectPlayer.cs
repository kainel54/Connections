using System;
using ObjectPooling;
using UnityEngine;

public class PoolingNoLifeTimeEffectPlayer : PoolingEffectPlayer
{
    [SerializeField] private EffectPoolingType _effectPoolingType;
    public override Enum PoolEnum => _effectPoolingType;
    
    public override void PlayEffect(Vector3 position, Quaternion rotation, Vector3 scale, Transform parant)
    {
        base.PlayEffect(position, rotation, scale, parant);
    }
}
