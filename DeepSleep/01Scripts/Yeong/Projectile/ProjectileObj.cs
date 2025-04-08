using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.StatSystem;
using ObjectPooling;
using YH.EventSystem;

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

        [field: SerializeField] public PoolingType PoolType { get; set; }
        public GameObject GameObject { get => gameObject; set { } }
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

        private void ApplyKnockBackToTarget(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IKnockBackable knockbackable))
            {
                Vector3 force = RbCompo.linearVelocity.normalized * _impactForce;
                Vector3 hitPoint = other.contacts[0].point;
                knockbackable.KnockBack(force, hitPoint);
            }
        }

        private void ApplyDamageToTarget(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                StatCompo statCompo = _owner.GetCompo<StatCompo>();
                damageable.ApplyDamage(statCompo, _damage);
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
    }

}
