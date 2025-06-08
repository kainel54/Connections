using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FallAttackRangeShow", story: "[Enemy] Show FallAttack Range", category: "Action", id: "b7022da4566b6514342c56bc09feaefe")]
public partial class FallAttackRangeShowAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Enemy;
    private SpinksEnemyAnimator _animator;
    protected override Status OnStart()
    {
        _animator = Enemy.Value.GetCompo<SpinksEnemyAnimator>();
        _animator.AttackRangeShower();
        return Status.Success;
    }
}

