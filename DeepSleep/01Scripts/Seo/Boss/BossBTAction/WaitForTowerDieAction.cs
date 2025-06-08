using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using YH.Enemy;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForTowerDie", story: "[Boss] Get Current Tower OnDie [Broken] Event", category: "Action", id: "0331560341817516b3a00c7284546fab")]
public partial class WaitForTowerDieAction : Action
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Boss;
    [SerializeReference] public BlackboardVariable<bool> Broken;

    private SpinksTowerManager _towerManager;
    private SpinksEnemyAttackCompo _attackCompo;
    protected override Status OnStart()
    {
        Broken.Value = false;

        _attackCompo = Boss.Value.GetCompo<SpinksEnemyAttackCompo>();
        _towerManager = _attackCompo.GetEnemyBossLevel().GetComponent<SpinksTowerManager>();

        _towerManager.GetCurrentTower().OnDieEvent.AddListener(HandleDieEvent);
        return Status.Running;
    }

    private void HandleDieEvent()
    {
        Broken.Value = true;
    }

    protected override Status OnUpdate()
    {
        return Broken.Value ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        _towerManager.GetCurrentTower().OnDieEvent.RemoveListener(HandleDieEvent);
    }
}


