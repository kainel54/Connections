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

    private EnemyMovement _movement;
    private float _maxDistance = 7;

    private float _lastChaseTime;
    public float calcPeriod = 0.1f;


    private float _elapsedTime = 0f;
    private float _duration = 1.1f;



    private float _speed;

    private Vector3 _endPos;
    private Vector3 _startPos;

    protected override Status OnStart()
    {
        _movement = Boss.Value.GetCompo<EnemyMovement>(true);

        _lastChaseTime = Time.time;
        _movement.SetStop(false);

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
        }
        _startPos = Boss.Value.transform.position;
        _endPos = _startPos + Boss.Value.transform.forward * distance;
        _speed = Vector3.Distance(Boss.Value.transform.position, _endPos) / (1.1f);


        _elapsedTime = 0f;

        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        _elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsedTime / 1.1f); // 정확히 1.1초
        float easet = EaseInOutQuad(t);
        Boss.Value.transform.position = Vector3.Lerp(_startPos, _endPos, easet);

        if (t >= 1f)
        {
            _movement.NavMeshEnable(true);
            return Status.Success;
        }

        return Status.Running;
    }

    private float EaseInOutQuad(float t)
    {
        return t < 0.5f
            ? 2f * t * t
            : -1f + (4f - 2f * t) * t;
    }
}

