using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetAnimationRootMotion", story: "[Animation] Set RootMotion [Boolean]", category: "Action", id: "90dff220b92c00bbe011ec77d3ffe769")]
public partial class SetAnimationRootMotionAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animation;
    [SerializeReference] public BlackboardVariable<bool> Boolean;

    protected override Status OnStart()
    {
        Animation.Value.applyRootMotion = Boolean.Value;
        Animation.Value.transform.localRotation = Quaternion.identity;
        return Status.Success;
    }
}