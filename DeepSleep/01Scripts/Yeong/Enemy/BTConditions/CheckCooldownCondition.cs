using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckCooldown", story: "[CoolTime] Filled in [PreviousTime]", category: "Conditions", id: "263ee7f31b92f108fffa71cfe95b1f1f")]
public partial class CheckCooldownCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CoolTime;
    [SerializeReference] public BlackboardVariable<float> PreviousTime;

    public override bool IsTrue()
    {
        if (PreviousTime.Value + CoolTime.Value <= Time.time || PreviousTime.Value == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
