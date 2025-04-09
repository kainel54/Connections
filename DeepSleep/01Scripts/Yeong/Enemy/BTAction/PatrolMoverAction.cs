using System;
using Unity.Behavior;
using UnityEngine;
using YH.Entities;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolMoverAction", story: "[Entity] Patrolling with [Mover] and [stopOffset]", category: "Action", id: "5b085834d8293fb3650c644f0bed06e0")]
public partial class PatrolMoverAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Entity;
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<float> stopOffset;

    private float _minMoveDis = 5f;
    private float _maxMoveDis = 15f;

    protected override Status OnStart()
    {
        Mover.Value.SetStop(false);

        Vector3 randPos = Random.insideUnitSphere * Random.Range(_minMoveDis, _maxMoveDis);
        randPos.y = 0;
        randPos += Entity.Value.transform.position;

        NavMesh.SamplePosition(randPos, out NavMeshHit hit, _maxMoveDis, 1);
        Mover.Value.SetDestination(hit.position);
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Mover.Value.GetIsArrived(stopOffset.Value))
            return Status.Success;

        return Status.Running;
    }
}

