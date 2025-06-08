using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChangingOverrideAnimator", story: "Change [Enemy] [Animator] to OverrideAnimator", category: "Action", id: "26c815e252753b75d53271ab4bb7f195")]
public partial class ChangingOverrideAnimatorAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<BTEnemy> Enemy;

    private SpinksTowerManager _towerManager;
    private SpinksEnemyAttackCompo _attackCompo;

    protected override Status OnStart()
    {
        _attackCompo = Enemy.Value.GetCompo<SpinksEnemyAttackCompo>();
        _towerManager = _attackCompo.GetEnemyBossLevel().GetComponent<SpinksTowerManager>();

        Animator.Value.runtimeAnimatorController = _towerManager.GetCurrentTower().GetOverrideAnimatior();

        return Status.Success;
    }

}

