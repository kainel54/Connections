using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using YH.Entities;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetVariable", story: "get variable from [entity]", category: "Action", id: "dc5a7dd970e28518c71b78721eec3e31")]
public partial class GetVariableAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Entity;

    protected override Status OnStart()
    {
        BTEnemy enemy = Entity.Value;
        
        enemy.SetVariable("Mover", enemy.GetCompo<EnemyMovement>());
        enemy.SetVariable("Renderer", enemy.GetCompo<EnemyAnimator>());
        enemy.SetVariable("AnimTrigger", enemy.GetCompo<EntityAnimationTrigger>());
        enemy.SetVariable("DamageCaster", enemy.GetCompo<EnemyDamageCaster>());
        enemy.SetVariable("Target", enemy.player);
        
        return Status.Running;
    }
}

