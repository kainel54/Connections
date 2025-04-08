using DG.Tweening;
using ObjectPooling;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PoolingEffectPlayer : MonoBehaviour, IPoolable
{
    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }


    private List<ParticleSystem> _particleEffect;

    private float _lifeDuration;

    private void Awake()
    {
        _particleEffect = GetComponentsInChildren<ParticleSystem>().ToList();
        _lifeDuration = _particleEffect[0].main.duration;
    }

    public void PlayEffect(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        _particleEffect.ForEach(x => x.Play());

        DOVirtual.DelayedCall(_lifeDuration, () => PoolManager.Instance.Push(this));
    }

    public void Init()
    {
        _particleEffect.ForEach(x =>
        {
            x.Stop();
            x.Simulate(0);
        });
    }
}
