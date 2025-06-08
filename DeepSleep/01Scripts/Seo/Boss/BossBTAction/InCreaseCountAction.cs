using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "InCreaseCount", story: "InCrease [Count] IsInCresase [IsIncrease]", category: "Action", id: "5010070b733384b4bf49a5259a6cce10")]
public partial class InCreaseCountAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Count;
    [SerializeReference] public BlackboardVariable<bool> IsIncrease;

    protected override Status OnStart()
    {
        if (IsIncrease)
            Count.Value += 1;
        else
            Count.Value -= 1;
        return Status.Success;
    }
}

