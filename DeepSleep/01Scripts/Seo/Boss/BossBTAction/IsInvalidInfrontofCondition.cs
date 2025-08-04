using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsValidInfrontof", story: "Something is Valid Infrontof This [Object] With [Length]", category: "Conditions", id: "a54d6b6ec02795067b37b9d28749854d")]
public partial class IsInvalidInfrontofCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<float> Length;

    public override bool IsTrue()
    {
        if (Physics.Raycast(Object.Value.transform.position + Vector3.up * 1.5f, Object.Value.transform.forward, Length.Value) == false)
            return true;

        return false;
    }
}
