using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ForceMove", story: "[Enemy] Move to Target for Animator Function ForceMove to Tower [Tower] or [Target]", category: "Action", id: "ebbc133a2b347fe9fc93fb7bac4b01da")]
public partial class ForceMoveAction : Action
{

    [SerializeReference] public BlackboardVariable<BTEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<bool> Tower;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    private SpinksEnemyAnimator _spinksAnimator;

    protected override Status OnStart()
    {
        _spinksAnimator = Enemy.Value.GetCompo<SpinksEnemyAnimator>();

        if (_spinksAnimator == null)
            return Status.Failure;




        if (Tower.Value)
            _spinksAnimator.ForceMoveToTower();
        else if (Target.Value != null)
            _spinksAnimator.ForceMoveToTarget(Target.Value.position);
        else
            _spinksAnimator.ForceMoveToFloor();



        return Status.Success;

    }
}

