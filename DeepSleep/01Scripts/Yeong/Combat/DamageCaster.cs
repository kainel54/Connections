using YH.Entities;
using UnityEngine;
using System.Collections.Generic;

namespace YH.Combat
{
    public abstract class DamageCaster : MonoBehaviour
    {
        [SerializeField] protected int _maxHitCount;
        protected Collider[] _hitResults;
        protected HashSet<Transform> _hitObjects;
        private void Awake()
        {
            _hitObjects = new HashSet<Transform>(_maxHitCount);
        }
        protected Entity _owner;
        public void StartCasting()
        {
            _hitObjects.Clear();
        }

        public virtual void InitCaster(Entity owner)
        {
            _hitResults = new Collider[_maxHitCount];
            _owner = owner;
        }

        public abstract bool CastDamage(float damage, Vector3 knockBack, bool isPowerAttack, LayerMask targetLayer);
        public abstract ICounterable GetCounterableTarget(LayerMask whatIsCounterable);
    }
}
