using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckTarget", story: "[entity] check [target] in [radius]", category: "Action", id: "b76a53eb5090fbb48340ee36824a554b")]
public partial class CheckTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Entity;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Radius;

    protected override Status OnStart()
    {
        Target.Value = Entity.Value.GetTargetInRadius(Radius.Value - 0.1f);
        return Target.Value == null ? Status.Failure : Status.Success;
    }
}

