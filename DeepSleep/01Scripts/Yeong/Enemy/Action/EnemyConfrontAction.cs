using System;
using Unity.Behavior;
using UnityEngine;
using YH.Entities;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using TMPro;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyConfront", story: "[Agent] confront to [Target] with [Mover] and [Attackrange]", category: "Action", id: "cf97af2e62f7c13f08b99852f4e9a295")]
public partial class EnemyConfrontAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<EnemyMovement> Mover;
    [SerializeReference] public BlackboardVariable<float> Attackrange;

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

        Agent.Value.FaceToTarget(Target.Value.position);
        if (_lastChaseTime + calcPeriod < Time.time)
        {
            Vector3 direction = (Agent.Value.transform.position - Target.Value.position).normalized;
            Vector3 nextPos = Target.Value.position + direction * Attackrange;
            Mover.Value.SetDestination(nextPos);
            _lastChaseTime = Time.time;
        }

        return Status.Running;
    }
}

