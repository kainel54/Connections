using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Setting timeAction", story: "Set [Currenttime]", category: "Action", id: "e3f88f5fa18c7a1ed551fd7e8e0ea09f")]
public partial class SettingTimeAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Currenttime;

    protected override Status OnStart()
    {
        Currenttime.Value = Time.time;
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

