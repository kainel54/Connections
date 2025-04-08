using System;
using UnityEngine;

namespace YH.Combat
{
    [CreateAssetMenu(fileName = "AttackDataSO", menuName = "SO/Combat/AttackData")]
    public class AttackDataSO : ScriptableObject
    {
        public string attackName;
        public Vector2 movement;
        public Vector2 knockBackForce;
        public float damageMultiplier;
        public float damageIncrease;
        public bool isPowerAttack;

        public float cameraShakePower;
        public float cameraShakeDuration;

        private void OnValidate()
        {
            attackName = this.name;
        }
    }
}
