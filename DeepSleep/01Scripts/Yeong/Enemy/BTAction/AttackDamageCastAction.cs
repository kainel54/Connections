using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackDamageCast", story: "Casting Damage with [EnemyDamageCaster] And [Animator] And [Active]", category: "Action", id: "76efbcc88a8b6e884d2cceaf045a1286")]
public partial class AttackDamageCastAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyDamageCaster> EnemyDamageCaster;
    [SerializeReference] public BlackboardVariable<EnemyAnimator> Animator;
    [SerializeReference] public BlackboardVariable<bool> Active;

    protected override Status OnStart()
    {
        if (Active.Value)
            Animator.Value.SetDamageCasterEvent += HandleDamageCasterEvent;
        else
            Animator.Value.SetDamageCasterEvent -= HandleDamageCasterEvent;

        return Status.Success;
    }

    private void HandleDamageCasterEvent(bool isActive)
    {
        EnemyDamageCaster.Value.SetDamageCaster(isActive);
    }

}

