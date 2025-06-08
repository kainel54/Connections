using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StateChange", message: "change to [state]", category: "Events", id: "d185975227ce5d3819dd4766e08d0d60")]
public partial class StateChange : EventChannelBase
{
    public delegate void StateChangeEventHandler(DefaultEnemyEnum state);
    public event StateChangeEventHandler Event; 

    public void SendEventMessage(DefaultEnemyEnum state)
    {
        Event?.Invoke(state);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<DefaultEnemyEnum> stateBlackboardVariable = messageData[0] as BlackboardVariable<DefaultEnemyEnum>;
        var state = stateBlackboardVariable != null ? stateBlackboardVariable.Value : default(DefaultEnemyEnum);
        Event?.Invoke(state);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        StateChangeEventHandler del = (state) =>
        {
            BlackboardVariable<DefaultEnemyEnum> var0 = vars[0] as BlackboardVariable<DefaultEnemyEnum>;
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
        Event += del as StateChangeEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as StateChangeEventHandler;
    }
}

