using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.StatSystem;
using ObjectPooling;
using YH.EventSystem;
using System;

namespace YH.Projectile
{
    public class ProjectileObj : MonoBehaviour, IPoolable
    {
        protected float _maxDistance, _distance;
        protected Vector3 _startPosition;
        public Rigidbody RbCompo { get; protected set; }
        protected float _impactForce, _damage;

        [Header("Event Channel")]
        [SerializeField] private GameEventChannelSO _spawnChannel;
        [SerializeField] private PoolingItemSO _impactItem;

        public GameObject GameObject { get => gameObject; set { } }

        public Enum PoolEnum { get => _type; set { } }
        [SerializeField] private ProjectileType _type;
        protected Entity _owner;

        protected virtual void Awake()
        {
            RbCompo = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            _distance = Vector3.Distance(transform.position, _startPosition);

            if (_distance >= _maxDistance)
            {
                PoolManager.Instance.Push(this);
            }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            CreateImpactFX(other);
            ApplyDamageToTarget(other);
            ApplyKnockBackToTarget(other);
            PoolManager.Instance.Push(this);
        }

        protected void ApplyKnockBackToTarget(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IKnockBackable knockbackable))
            {
                Vector3 force = RbCompo.linearVelocity.normalized * _impactForce;
                Vector3 hitPoint = other.contacts[0].point;
                knockbackable.KnockBack(force, hitPoint);
            }
        }

        protected void ApplyDamageToTarget(Collision other)
        {
            if (_owner is BTEnemy && other.gameObject.layer == LayerMask.NameToLayer("Tower")) return;

            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                EntityStat statCompo = _owner.GetCompo<EntityStat>();
                HitData hitData = new HitData(_owner, _damage,
                    statCompo.GetElement("Critical").Value,
                    statCompo.GetElement("CriticalDamage").Value);

                damageable.ApplyDamage(hitData, true, true, 1);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
            {
                Debug.Log("Shield Hit");
                EntityStat statCompo = _owner.GetCompo<EntityStat>();
                HitData hitData = new HitData(_owner, _damage,
                    statCompo.GetElement("Critical").Value,
                    statCompo.GetElement("CriticalDamage").Value);

                other.transform.GetComponent<Shield>().ApplyDamage(hitData);
            }
        }

        private void CreateImpactFX(Collision other)
        {
            if (other.contacts.Length > 0)
            {
                ContactPoint contact = other.GetContact(0);
                var evt = SpawnEvents.EffectSpawn;
                evt.effectItem = _impactItem;
                evt.position = contact.point;
                evt.rotation = Quaternion.LookRotation(contact.normal);
                evt.scale = new Vector3(0.2f, 0.2f, 0.2f);
                _spawnChannel.RaiseEvent(evt);
            }
        }
        public virtual void Fire(Vector3 position, Quaternion rotation, BulletPayload payload, Entity owner)
        {
            transform.SetPositionAndRotation(position, rotation);
            RbCompo.mass = payload.mass;
            RbCompo.linearVelocity = payload.velocity;
            _maxDistance = payload.shootingRange;
            _impactForce = payload.impactForce;
            _damage = payload.damage;
            _startPosition = position;
            _owner = owner;
        }

        public virtual void Init()
        {
        }

        public void OnPop()
        {
            Init();
        }

        public void OnPush()
        {

        }
    }

}
