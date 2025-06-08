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

    [SerializeField] private float wallCheckRadius = 0.25f;

    private LayerMask _whatIsWall;
    private EnemyMovement _movement;
    private Vector3 _targetPosition;
    private float _moveSpeed;
    private float _startTime;

    protected override Status OnStart()
    {
        _movement = Agent.Value.GetCompo<EnemyMovement>();

        Vector3 direction = Target.Value.position - Agent.Value.transform.position;
        direction.y = 0;
        direction.Normalize();

        Vector3 start = Agent.Value.transform.position;
        RaycastHit hit;
        _whatIsWall = LayerMask.GetMask("Obstacle", "InvisibleWall", "Wall");
        // ���� �ε����� ���� Ȯ��
        if (Physics.SphereCast(start, wallCheckRadius, direction, out hit, Distance, _whatIsWall))
        {
            _targetPosition = hit.point;
            float actualDistance = Vector3.Distance(start, _targetPosition);
            _moveSpeed = actualDistance / Duration;
        }
        else
        {
            _targetPosition = start + direction * Distance;
            _moveSpeed = Distance / Duration;
        }

        _startTime = Time.time;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.position = Vector3.MoveTowards(
            Agent.Value.transform.position,
            _targetPosition,
            _moveSpeed * Time.deltaTime
        );

        if (Time.time >= _startTime + Duration ||
            Vector3.Distance(Agent.Value.transform.position, _targetPosition) < 0.05f)
        {
            return Status.Success;
        }

        return Status.Running;
    }

}

