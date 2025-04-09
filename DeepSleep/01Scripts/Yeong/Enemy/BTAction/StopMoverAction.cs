using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using YH.Entities;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopMover", story: "stop [mover] [On]", category: "Action", id: "d358031ecd61fd2c230ae9ed27c6cbbb")]
public partial class StopMoverAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<bool> On;
    protected override Status OnStart()
    {
        Mover.Value.SetStop(On.Value);
        return Status.Success;
    }
}

