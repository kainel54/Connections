using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Count", story: "Check [Count] [Value]", category: "Conditions", id: "e4837639d96e52736ea5f2fc9ab2c140")]
public partial class CheckCountCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> Count;
    [SerializeReference] public BlackboardVariable<float> Value;

    public override bool IsTrue()
    {
        if (Count.Value > Value.Value)
        {
            Count.Value = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void OnStart()
    {
        Count.Value++;
    }

    public override void OnEnd()
    {
    }
}
