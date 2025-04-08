using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "InCreaseTimer", story: "Increase [Timer] of [SkillCoolTime]", category: "Action", id: "4554e76e114a5727f3adf09d0009d54a")]
public partial class InCreaseTimerAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Timer;
    [SerializeReference] public BlackboardVariable<float> SkillCoolTime;
    protected override Status OnUpdate()
    {
        if (SkillCoolTime <= 0)
        {
            return Status.Failure;
        }

        if (SkillCoolTime.Value <= Timer.Value)
        {
            Timer.Value = SkillCoolTime.Value;
            return Status.Success;
        }

        Timer.Value += Time.deltaTime;

        return Status.Running;
    }

}

