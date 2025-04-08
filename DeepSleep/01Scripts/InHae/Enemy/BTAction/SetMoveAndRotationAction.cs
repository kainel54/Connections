using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Serialization;
using YH.Entities;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetMoveAndRotation", story: "[Entity] Move And Rotation with [Mover] and [animationTrigger]", category: "Action", id: "3832014fe1664b1d79d5a76a83c90a44")]
public partial class SetMoveAndRotationAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Entity;
    [SerializeReference] public BlackboardVariable<EnemyMovement> Movement;
    [SerializeReference] public BlackboardVariable<EntityAnimationTrigger> AnimationTrigger;

    private bool _isTriggered;
    
    protected override Status OnStart()
    {
        _isTriggered = false;
        AnimationTrigger.Value.OnAnimationEndTrigger += HandleAnimationEnd;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Movement.Value.IsManualRotation)
        {
            // Vector3 moveDelta = Entity.Value.transform.forward * MAX_ATK_DISTANCE;
            // _attackDestination = _startPosition + moveDelta;
            Entity.Value.FaceToTarget(Entity.Value.player.transform.position);
        }

        if (Movement.Value.IsManualMove)
        {
            // _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, 
            //     _attackDestination, _weaponController.attackData.moveSpeed * Time.deltaTime);
        }

        if (_isTriggered)
            return Status.Success;
        
        return Status.Running;
    }
    
    protected override void OnEnd()
    {
        AnimationTrigger.Value.OnAnimationEndTrigger -= HandleAnimationEnd;
    }
    
    private void HandleAnimationEnd()
    {
        _isTriggered = true;
    }
}

