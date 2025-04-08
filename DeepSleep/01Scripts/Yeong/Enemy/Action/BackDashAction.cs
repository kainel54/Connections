using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using YH.Entities;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BackDash", story: "[Agent] BackJump to [Duration]", category: "Action", id: "52833fdebd9384c224aa124d7823aaa0")]
public partial class BackDashAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Agent;
    [SerializeReference] public BlackboardVariable<float> Duration;
    [SerializeReference] public BlackboardVariable<float> Distance;
    private EnemyMovement _movement;

    private Vector3 _targetPosition;
    private float _moveSpeed;
    private float _startTime;

    protected override Status OnStart()
    {
        _movement = Agent.Value.GetCompo<EnemyMovement>();

        // ���� ��ġ�� ���� ���� ���
        Vector3 backwardDir = -Agent.Value.transform.forward;
        Vector3 origin = Agent.Value.transform.position;

        // �ڷ� Raycast
        RaycastHit hit;
        Vector3 targetPos;

        if (Physics.Raycast(origin, backwardDir, out hit, Distance , Agent.Value.whatIsWall))
        {
            // �浹�� ��ġ������ �̵�
            targetPos = hit.point;
        }
        else
        {
            // �ƹ��͵� �� �¾����� Distance��ŭ �ڷ� �̵�
            targetPos = origin + backwardDir * Distance;
        }

        // y�� ����
        targetPos.y = origin.y;

        // ����
        _targetPosition = targetPos;
        Debug.Log(_targetPosition);
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

    protected override void OnEnd()
    {
    }
}

