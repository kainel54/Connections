using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Health Check", story: "[Owner] check Health [Percent]", category: "Conditions", id: "2c4376c93001d0ab82e01fb274625fbc")]
public partial class HealthCheckCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BTEnemy> Owner;
    [SerializeReference] public BlackboardVariable<float> Percent;

    public override bool IsTrue()
    {
        EntityHealth healthCompo = Owner.Value.GetCompo<EntityHealth>();
        float health = healthCompo.Health;
        float maxHealth = healthCompo.MaxHealth;
        if (health / maxHealth * 100 > Percent.Value)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
