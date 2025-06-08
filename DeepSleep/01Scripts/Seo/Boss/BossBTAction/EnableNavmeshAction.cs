using System;
using Unity.Behavior;
using UnityEngine;
using YH.Entities;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Enable Navmesh", story: "[Movement] Navmesh Enable [Value]", category: "Action", id: "de83e765e2eca5f9a2c87b6e76129ca1")]
public partial class EnableNavmeshAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyMovement> Movement;
    [SerializeReference] public BlackboardVariable<bool> Value;

    protected override Status OnStart()
    {
        Movement.Value.NavMeshEnable(Value.Value);
        return Status.Success;
    }

}

