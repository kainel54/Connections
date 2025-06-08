using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CoolDown", story: "[TargetTime] to [IncreaseTime]", category: "Action", id: "2bf51e62fd85a1b87505f8802ebf7605")]
public partial class CoolDownAction : Action
{
    [SerializeReference] public BlackboardVariable<float> TargetTime;
    [SerializeReference] public BlackboardVariable<float> IncreaseTime;

    protected override Status OnStart()
    {
        if (TargetTime == null || IncreaseTime == null)
        {
            return Status.Failure;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (TargetTime.Value > IncreaseTime.Value)
        {
            IncreaseTime.Value+= Time.deltaTime;
            return Status.Running;
        }
        else
        {
            return Status.Success;
        }
    }

    protected override void OnEnd()
    {
    }
}

