using System;
using YH.Entities;
using UnityEngine;

namespace YH.Entities
{
    public class EntityAnimationTrigger : MonoBehaviour, IEntityComponent
    {
        public event Action OnAnimationEndTrigger;
        public event Action<bool> OnAttackTrigger;
        
        protected Entity _entity;
        
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;    
        }
        protected virtual void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
        protected virtual void AttackOnTrigger()
        {
            OnAttackTrigger?.Invoke(true);
        }

        protected virtual void AttackOffTrigger()
        {
            OnAttackTrigger?.Invoke(false);
        }
    }
}
