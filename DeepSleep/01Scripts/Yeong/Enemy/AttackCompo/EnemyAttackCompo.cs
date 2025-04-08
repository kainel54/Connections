using ObjectPooling;
using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Entities;
using YH.EventSystem;

namespace YH.Enemy
{
    public abstract class EnemyAttackCompo : MonoBehaviour, IEntityComponent
    {
        protected BTEnemy _enemy;
        
        public void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;
        }
    }
}

