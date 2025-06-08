using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AppUI.Core;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.EventSystem;
using YH.StatSystem;

public class SwordObj : MonoBehaviour, IPoolable
{
    public bool AttackToDot = false;
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

    private Entity _owner;

    private float _lifeDuration;
    private float _damage;

    protected virtual void Awake()
    {
        RbCompo = GetComponent<Rigidbody>();
        _particleEffect = GetComponentsInChildren<ParticleSystem>().ToList();
        _lifeDuration = .15f;
    }

    public void PlayEffect(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = scale;
        _particleEffect.ForEach(x => x.Play());

        DOVirtual.DelayedCall(_lifeDuration, () => PoolManager.Instance.Push(this, true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (AttackToDot)
            ApplyDamageDot(other);
        else
            ApplyDamageToTarget(other);
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

    private void ApplyDamageDot(Collider other)
    {
        Vector3 dir = (other.transform.position - transform.position).normalized;

        float dot = Vector3.Dot(dir, transform.forward);
        float anglelimit = Mathf.Cos(300 * 0.5f * Mathf.Deg2Rad);
        if (dot >= anglelimit)
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
    }
    public void Init()
    {

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
