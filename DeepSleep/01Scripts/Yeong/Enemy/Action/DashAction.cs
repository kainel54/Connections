using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using YH.Entities;
using System.Threading.Tasks;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DashAction", story: "[Agent] Dash to [Target] for [Distance] and [Duration]", category: "Action", id: "967982ba6b9d8cabc95bdc6ee0f4961c")]
public partial class DashAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<float> Duration;
    private EnemyMovement _movement;

    private Vector3 _targetPosition;
    private float _moveSpeed;
    private float _startTime;

    protected override Status OnStart()
    {
        _movement = Agent.Value.GetCompo<EnemyMovement>();
        Vector3 targetPos = Target.Value.position - Agent.Value.transform.position;
        targetPos.y = 0;
        _targetPosition = Agent.Value.transform.position + Vector3.Normalize(targetPos) * Distance;
        Debug.Log(_targetPosition * Distance);
        _moveSpeed = Distance / Duration;
        _startTime = Time.time;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.position = Vector3.MoveTowards(Agent.Value.transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
        if (_startTime + Duration < Time.time)
        {

            return Status.Success;
        }
        return Status.Running;
    }

}

