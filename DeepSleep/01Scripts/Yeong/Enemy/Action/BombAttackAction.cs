using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BombAttack", story: "[Self] [animator] Bomb Attack", category: "Action", id: "413ae480a6cdf2da1ace8ee4b34e1f88")]
public partial class BombAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<SelfBombEnemyAnimator> Animator;

    protected override Status OnStart()
    {
        if (Animator.Value.bombDisplayCnt >= 1)
        {
            return Status.Success;
        }
        Animator.Value.SelfBombDisplaySetting();
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }
}

