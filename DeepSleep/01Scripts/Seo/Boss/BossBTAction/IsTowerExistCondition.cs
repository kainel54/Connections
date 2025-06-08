using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsTowerExist", story: "Is [Boss] SpinksTowerExist", category: "Conditions", id: "1b075a170265cc833a1f54fec798d042")]
public partial class IsTowerExistCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Boss;

    private SpinksTowerManager _spinksTower;
    public override bool IsTrue()
    {
        return _spinksTower.CanGetAliveTower();
    }

    public override void OnStart()
    {
        _spinksTower = Boss.Value.GetCompo<SpinksEnemyAttackCompo>()
            .GetEnemyBossLevel().GetComponentInChildren<SpinksTowerManager>();
    }

    public override void OnEnd()
    {
    }
}
