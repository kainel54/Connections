using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Energy Deducted", story: "[Energy] Deducted [Value]", category: "Action", id: "3914409a434698c52d1a496357b12be1")]
public partial class EnergyDeductedAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Energy;
    [SerializeReference] public BlackboardVariable<float> Value;

    protected override Status OnStart()
    {
        Energy.Value -= Value.Value;
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

