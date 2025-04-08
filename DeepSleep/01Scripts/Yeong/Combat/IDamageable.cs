using YH.Entities;
using UnityEngine;
using YH.StatSystem;

namespace YH.Combat
{
    public interface IDamageable
    {
        //todo 여기도 데미지 구조체를 받는 형태로 변경된다.
        public void ApplyDamage(StatCompo statCompo, float damage, bool isChangeVisible = true, bool isTextVisible = true);
    }
}
