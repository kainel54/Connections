using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Energy Check", story: "More [energy] than [required]", category: "Conditions", id: "6839db47f4ade95055a5600342fb8721")]
public partial class EnergyCheckAction : Condition
{
    [SerializeReference] public BlackboardVariable<float> Energy;
    [SerializeReference] public BlackboardVariable<float> Required;

    public override bool IsTrue()
    {
        if (Energy.Value >= Required.Value)
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
