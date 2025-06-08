using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetInvincible", story: "Set [Enemy] Invincible Mode [bool]", category: "Action", id: "3dc9f0cf9173499262311c801b2d4f27")]
public partial class SetInvincibleAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<bool> Bool;

    protected override Status OnStart()
    {
        if (Enemy == null)
            return Status.Failure;

        Enemy.Value.Setinvincible(Bool.Value);
        return Status.Success;
    }
}

