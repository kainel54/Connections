using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPooling;
using UnityEngine;

public abstract class PoolingEffectPlayer : MonoBehaviour, IPoolable
{
    private List<ParticleSystem> _particleEffect;
    protected float _lifeDuration;

    private void Awake()
    {
        _particleEffect = GetComponentsInChildren<ParticleSystem>().ToList();
        _lifeDuration = _particleEffect[0].main.duration;
    }

    public virtual void PlayEffect(Vector3 position, Quaternion rotation, Vector3 scale, Transform parant)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = scale;
        transform.SetParent(parant);
        _particleEffect.ForEach(x => x.Play());
    }

    public void SetDuration(float duration)
    {
        _lifeDuration = duration;
    }

    public void Init()
    {
        _particleEffect.ForEach(x =>
        {
            x.Stop();
            x.Simulate(0);
        });
    }

    public GameObject GameObject => gameObject;
    public virtual Enum PoolEnum { get; }

    public virtual void OnPop()
    {
        Init();
    }

    public virtual void OnPush()
    {
        Init();
    }

    public void SetLifeTime(float duration)
    {
        _particleEffect.ForEach(x =>
        {
            var main = x.main;
            main.startLifetime = duration;
        });
    }
}
