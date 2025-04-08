using YH.Entities;
using UnityEngine;

namespace YH.Combat
{
    public class EntityFeedbackData : MonoBehaviour, IEntityComponent
    {
        #region Hit by other data
        [field: SerializeField] public bool IsLastHitCritical { get; set; } = false;
        [field: SerializeField] public Vector2 LastAttackDirection { get; set; }
        [field: SerializeField] public bool IsLastHitPowerAttack { get; set; }
        [field: SerializeField] public Entity LastEntityWhoHit { get; set; }
        #endregion
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
    }
}
