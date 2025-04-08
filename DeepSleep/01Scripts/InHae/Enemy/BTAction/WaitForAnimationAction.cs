using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using YH.Entities;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForAnimation", story: "wait for [animationTrigger]", category: "Action", id: "84a18126337bd5ce3db11ddf3696be46")]
public partial class WaitForAnimationAction : Action
{
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
        return _isTriggered ? Status.Success : Status.Running;
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

