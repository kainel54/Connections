using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using YH.Entities;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetNavmeshWarp", story: "[Enemy] Move to [Animator] position [Boolean]", category: "Action", id: "5f87463e3a27691374a4476d8055558c")]
public partial class SetNavmeshWarpAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<bool> Boolean;

    private EnemyMovement _mover;
    protected override Status OnStart()
    {
        _mover = Enemy.Value.GetCompo<EnemyMovement>();
        if (Boolean.Value)
        {
            _mover.SetStartingWarpSetting();
        }
        else
        {
            _mover.SetNavAgentWarp(Animator.Value.rootPosition, Animator.Value.rootRotation);
            Animator.Value.transform.localPosition = Vector3.zero;
        }

        return Status.Success;
    }
}

