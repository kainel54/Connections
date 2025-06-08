using System.Collections.Generic;
using UnityEngine;
using YH.Core;
using YH.StatSystem;

namespace YH.Combat
{
    public class OverlapCircleDamageCaster : DamageCaster
    {
        public float damageRadius;
        public Vector3 damagePosition;

        public override bool CastDamage(float damage, Vector3 knockBack, bool isPowerAttack,LayerMask targetLayer)
        {
            int cnt = Physics.OverlapSphereNonAlloc(transform.position + damagePosition, damageRadius, _hitResults,targetLayer);

            
            for (int i = 0; i < cnt; i++)
            {
                Transform target = _hitResults[i].transform;

                if (_hitObjects.Contains(target.root))
                    continue;
            
                _hitObjects.Add(target.root);

                if (knockBack != Vector3.zero)
                {
                    Vector2 direction = (_hitResults[i].transform.position - _owner.transform.position).normalized;
                    knockBack.x *= Mathf.Sign(direction.x);
                }

                if (_hitResults[i].TryGetComponent(out IDamageable damageable))
                {
                    CameraManager.Instance.ShakeCamera(4, 4, 0.15f);
                    
                    
                    EntityStat statCompo = _owner.GetCompo<EntityStat>();
                    HitData hitData = new HitData(_owner, damage, 
                        statCompo.GetElement("Critical").Value, 
                        statCompo.GetElement("CriticalDamage").Value);
                    damageable.ApplyDamage(hitData);
                }
                //todo 나중에 넉백도 적용
            }

            return cnt > 0;
        }

        public override ICounterable GetCounterableTarget(LayerMask whatIsCounterable)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, damageRadius, whatIsCounterable);
            
            if(collider != null)
                return collider.GetComponent<ICounterable>();

            return default;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position + damagePosition, damageRadius);
        }
#endif
    }
}
