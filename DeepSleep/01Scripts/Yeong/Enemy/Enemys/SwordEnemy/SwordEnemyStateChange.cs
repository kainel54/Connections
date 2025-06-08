using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/SwordEnemyStateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "SwordEnemyStateChange", message: "change to [state]", category: "Events", id: "d185975227ce5d3819dd4766e08d0d60")]
public partial class SwordEnemyStateChange : EventChannelBase
{
    public delegate void SwordEnemyStateChangeEventHandler(SwordEnemyEnum state);
    public event SwordEnemyStateChangeEventHandler Event;

    public void SendEventMessage(SwordEnemyEnum state)
    {
        Event?.Invoke(state);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<SwordEnemyEnum> stateBlackboardVariable = messageData[0] as BlackboardVariable<SwordEnemyEnum>;
        var state = stateBlackboardVariable != null ? stateBlackboardVariable.Value : default(SwordEnemyEnum);
        Event?.Invoke(state);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        SwordEnemyStateChangeEventHandler del = (state) =>
        {
            BlackboardVariable<SwordEnemyEnum> var0 = vars[0] as BlackboardVariable<SwordEnemyEnum>;
            if (var0 != null)
            {
                var0.Value = state;
            }

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as SwordEnemyStateChangeEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as SwordEnemyStateChangeEventHandler;
    }
}
