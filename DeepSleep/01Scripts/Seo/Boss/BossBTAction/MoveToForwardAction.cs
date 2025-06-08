using System;
using Unity.Behavior;
using UnityEngine;
using YH.Entities;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor;
using Unity.AppUI.UI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToForward", story: "[Boss] Move To Forward", category: "Action", id: "e4a0861911331b57dcac009a50f41a82")]
public partial class MoveToForwardAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Boss;

    private EnemyMovement _movemet;
    private float _maxDistance = 7;

    private float _lastChaseTime;
    public float calcPeriod = 0.1f;

    Vector3 endPos;

    protected override Status OnStart()
    {
        _movemet = Boss.Value.GetCompo<EnemyMovement>();

        _lastChaseTime = Time.time;
        _movemet.SetStop(false);

        float distance;
        if (Physics.Raycast(Boss.Value.transform.position + Vector3.up * 2f, Boss.Value.transform.forward, out RaycastHit hit, 7f, Boss.Value.whatIsWall))
        {
            Vector3 hitPos = new Vector3(hit.point.x, hit.point.y - 2f, hit.point.z);
            hitPos = hitPos - Boss.Value.transform.forward;
            distance = Vector3.Distance(hitPos, Boss.Value.transform.position);
        }
        else
        {
            distance = _maxDistance;
            Debug.Log("레이 안 맞음");
        }
        endPos = Boss.Value.transform.position + Boss.Value.transform.forward * distance;

        _movemet.SetDestination(endPos);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        Boss.Value.FaceToTarget(_movemet.GetNextPathPoint());
        if (_lastChaseTime + calcPeriod < Time.time)
        {
            _movemet.SetDestination(endPos);
            _lastChaseTime = Time.time;
        }
        _movemet.GetNextPathPoint();



        if (Vector3.Distance(endPos, Boss.Value.transform.position) < 0.2f)
            return Status.Success;

        return Status.Running;
    }
    protected override void OnEnd()
    {
        _movemet.SetSpeed(20);

    }
}

