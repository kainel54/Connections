using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/ThrowEnemyStateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "ThrowEnemyStateChange", message: "change to [state]", category: "Events", id: "d185975227ce5d3819dd4766e08d0d60")]
public partial class ThrowEnemyStateChange : EventChannelBase
{
    public delegate void ThrowEnemyStateChangeEventHandler(ThrowEnemyEnum state);
    public event ThrowEnemyStateChangeEventHandler Event;

    public void SendEventMessage(ThrowEnemyEnum state)
    {
        Event?.Invoke(state);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<ThrowEnemyEnum> stateBlackboardVariable = messageData[0] as BlackboardVariable<ThrowEnemyEnum>;
        var state = stateBlackboardVariable != null ? stateBlackboardVariable.Value : default(ThrowEnemyEnum);
        Event?.Invoke(state);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        ThrowEnemyStateChangeEventHandler del = (state) =>
        {
            BlackboardVariable<ThrowEnemyEnum> var0 = vars[0] as BlackboardVariable<ThrowEnemyEnum>;
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
        Event += del as ThrowEnemyStateChangeEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as ThrowEnemyStateChangeEventHandler;
    }
}
