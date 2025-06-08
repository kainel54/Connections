using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.EventSystem;
using YH.StatSystem;

public class LightningBeam : MonoBehaviour, IPoolable
{
    private Vector3 _startPosition;
    public Rigidbody RbCompo { get; protected set; }
    public GameObject GameObject { get => gameObject; set { } }
    public Enum PoolEnum { get => _type; set { } }
    [SerializeField] private ProjectileType _type;

    [Header("Event Channel")]
    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private PoolingItemSO _impactItem;

    [SerializeField] private StatElementSO _damageElement;
    private List<ParticleSystem> _particleEffect;
    private float _lifeDuration;
    private float _damage;
    private float _lastHitTime;
    
    private Entity _owner;

    private void Awake()
    {
        RbCompo = GetComponent<Rigidbody>();
        _particleEffect = GetComponentsInChildren<ParticleSystem>().ToList();
        _lifeDuration = _particleEffect[0].main.duration;
    }

    public void PlayEffect(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = scale;
        _particleEffect.ForEach(x => x.Play());

        DOVirtual.DelayedCall(_lifeDuration, () => PoolManager.Instance.Push(this,true));
    }

    public void Init()
    {
        _particleEffect.ForEach(x =>
        {
            x.Stop();
            x.Simulate(0);
        });

    }

    private void OnTriggerStay(Collider other)
    {
        if (_lastHitTime + 0.5f > Time.time)
            return;
        ApplyDamageToTarget(other);
        CreateImpactFX(other);
        _lastHitTime = Time.time;
    }


    private void ApplyDamageToTarget(Collider other)
    {
        
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            EntityStat statCompo = _owner.GetCompo<EntityStat>();
            HitData hitData = new HitData(_owner, _damage,
                statCompo.GetElement("Critical").Value,
                statCompo.GetElement("CriticalDamage").Value);

            damageable.ApplyDamage(hitData);
        }
    }
    private void CreateImpactFX(Collider other)
    {
            var evt = SpawnEvents.EffectSpawn;
            evt.effectItem = _impactItem;
            evt.position = other.transform.position;
            evt.rotation = Quaternion.LookRotation(other.transform.position.normalized);
            evt.scale = new Vector3(0.2f, 0.2f, 0.2f);
            _spawnChannel.RaiseEvent(evt);
    }

    public void Setting(Entity owner)
    {
        _owner = owner;
        _damage = _owner.GetCompo<EntityStat>().GetElement(_damageElement).Value;
    }

    public void OnPop()
    {
    }

    public void OnPush()
    {
    }
}
