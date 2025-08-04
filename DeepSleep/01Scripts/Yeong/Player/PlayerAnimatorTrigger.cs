using YH.Entities;
using UnityEngine;
using System;

namespace YH.Players
{
    public class PlayerAnimatorTrigger : EntityAnimationTrigger
    {
        public event Action OnDashTrigger;
        public event Action<float> OnMoveForwardTrigger;
        public event Action<float> OnMoveYTrigger;
        public event Action OnAttackEnableTrigger;
        public event Action<int> OnSkillActiveTrigger;
        public event Action<int> OnDashActiveTrigger;

        public void DashTrigger()
        {
            Debug.Log("Dash Triggered");
            OnDashTrigger?.Invoke();
        }

        public void MoveForwardTrigger(float fowardDirection)
        {
            OnMoveForwardTrigger?.Invoke(fowardDirection);
        }
        public void MoveYTrigger(float yDirection)
        {
            OnMoveYTrigger?.Invoke(yDirection);
        }

        public void AttackEnableTrigger() => OnAttackEnableTrigger?.Invoke();
        public void SkillActiveTrigger(int number) => OnSkillActiveTrigger?.Invoke(number);
        
        // bool 값처럼 활용 0 == false, 1 == true
        public void DashActiveTrigger(int isEnable) => OnDashActiveTrigger?.Invoke(isEnable);
    }
}
