using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using YH.Entities;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "[entity] move to [target] with [mover] and [stopOffset]", category: "Action", id: "57d71d3dcf8eef1a4bdfaf91873a082d")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Entity;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<float> stopOffset;

    private float _lastChaseTime;
    public float calcPeriod = 0.1f;

    protected override Status OnStart()
    {
        _lastChaseTime = Time.time;
        Mover.Value.SetStop(false);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Target.Value == null)
            return Status.Failure;

        Entity.Value.FaceToTarget(Mover.Value.GetNextPathPoint());
        if (_lastChaseTime + calcPeriod < Time.time)
        {
            Mover.Value.SetDestination(Target.Value.position);
            _lastChaseTime = Time.time;
        }

        if (Vector3.Distance(Target.Value.transform.position, Mover.Value.transform.position) < stopOffset)
            return Status.Success;


        return Status.Running;
    }
}

