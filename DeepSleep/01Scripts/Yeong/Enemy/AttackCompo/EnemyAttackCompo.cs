using ObjectPooling;
using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Entities;
using YH.EventSystem;

namespace YH.Enemy
{
    public abstract class EnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitable
    {
        protected BTEnemy _enemy;
        protected Transform _magicFallShootTrm;
        public virtual void AfterInit()
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;
        }


        public void SetBossLevel()
        {
            IGetBossLevelAble bossLevelAble = _enemy as IGetBossLevelAble;
            if (bossLevelAble != null)
            {
                BossLevelRoom bossLevel = bossLevelAble.GetBossLevel();
                _magicFallShootTrm = bossLevel.transform.Find("SpawnPoint").transform;
            }
        }

        public BossLevelRoom GetEnemyBossLevel()
        {
            IGetBossLevelAble bossLevelAble = _enemy as IGetBossLevelAble;
            if (bossLevelAble != null)
            {
                BossLevelRoom bossLevel = bossLevelAble.GetBossLevel();
                return bossLevel;
            }
            return null;
        }
    }
}

