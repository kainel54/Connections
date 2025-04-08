using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Energy Charging", story: "[Energy] Charging [amount]", category: "Action", id: "b3ec236c5ae725d881f9338c70c9bdc6")]
public partial class EnergyChargingAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Energy;
    [SerializeReference] public BlackboardVariable<float> Amount;
    private float _startTime;
    public float duration;

    protected override Status OnStart()
    {
        _startTime = Time.time;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

