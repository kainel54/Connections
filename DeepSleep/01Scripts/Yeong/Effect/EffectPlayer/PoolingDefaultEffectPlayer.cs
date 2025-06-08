using System;
using DG.Tweening;
using ObjectPooling;
using UnityEngine;

public class PoolingDefaultEffectPlayer : PoolingEffectPlayer
{
    [SerializeField] private EffectPoolingType _effectPoolingType;
    public override Enum PoolEnum => _effectPoolingType;

    public override void PlayEffect(Vector3 position, Quaternion rotation, Vector3 scale, Transform parant)
    {
        base.PlayEffect(position, rotation, scale, parant);
        DOVirtual.DelayedCall(_lifeDuration, () =>
        {
            PoolManager.Instance.Push(this, true);
        });
    }
}