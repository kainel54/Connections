using YH.Entities;
using UnityEngine;
using YH.StatSystem;

namespace YH.Combat
{
    public struct HitData
    {
        public HitData(Entity _entity, float _damage, float _ciriticalChance, float _ciriticalDamage)
        {
            attacker = _entity;
            damage = _damage;
            ciriticalChance = _ciriticalChance;
            ciriticalDamage = _ciriticalDamage;
        }
        
        public Entity attacker;
        public float damage;
        public float ciriticalChance;
        public float ciriticalDamage;
    }
    public interface IDamageable
    {
        //todo 여기도 데미지 구조체를 받는 형태로 변경된다.
        public void ApplyDamage(HitData hitData, bool isChangeVisible = true, bool isTextVisible = true,float damageDecrease = 1);
    }
}
