using YH.Entities;
using UnityEngine;

namespace YH.Combat
{
    public interface ICounterable
    {
        public bool CanCounter { get; }
        public void ApplyCounter(float damage, Vector2 direction, Vector2 knockBack, 
                                bool isPowerAttack, Entity dealer);
    }
}
