using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CanAttackToTower", story: "[Boss] Player Can Tower Attack [Bool]", category: "Action", id: "4658627c012bf165925d160a92fbfedd")]
public partial class CanAttackToTowerAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Boss;
    [SerializeReference] public BlackboardVariable<bool> Bool;

    private SpinksTowerManager _spinksTower;

    protected override Status OnStart()
    {
        _spinksTower = Boss.Value.GetCompo<SpinksEnemyAttackCompo>()
            .GetEnemyBossLevel().GetComponentInChildren<SpinksTowerManager>();

        if (Bool.Value)
            _spinksTower.CanAttackToTower();
        else
            _spinksTower.CantAttackToTower();

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

