using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "RaycastHit", story: "Raycasting [Start] to [Distance] and Hit [Target] Obj", category: "Conditions", id: "2f08df4e628e8b8a487448670bdcec1b")]
public partial class RaycastHitCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Start;
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    public override bool IsTrue()
    {
        RaycastHit hit;
        if (Physics.Raycast(Start.Value.position + Vector3.up * 3, Start.Value.forward, out hit, 3f))
        {
            if (hit.transform == Target.Value)
                return true;
        }

        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
