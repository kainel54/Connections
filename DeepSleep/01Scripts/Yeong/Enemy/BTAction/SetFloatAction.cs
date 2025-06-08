using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetFloat", story: "[Set] [Float]", category: "Action", id: "5853e713cbbe94ae13fd693da5641d24")]
public partial class SetFloatAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Set;
    [SerializeReference] public BlackboardVariable<float> Float;

    protected override Status OnStart()
    {
        Set.Value = Float.Value;
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

