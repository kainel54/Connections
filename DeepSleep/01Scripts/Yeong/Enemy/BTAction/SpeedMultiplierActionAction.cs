using System;
using Unity.Behavior;
using UnityEngine;
using YH.Entities;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpeedMultiplierAction", story: "[Mover] speed [multiplier]", category: "Action", id: "d4d147469bce0d722c5fd0b857e74398")]
public partial class SpeedMultiplierActionAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<float> Multiplier;

    protected override Status OnStart()
    {
        Mover.Value.SetSpeedMultiplier(Multiplier.Value);
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

