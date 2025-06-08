using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/SpinksStateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "SpinksStateChange", message: "Spinks State Assign to [State]", category: "Events", id: "0703ba0a204c161440bfa9004fc3b531")]
public partial class SpinksStateChange : EventChannelBase
{
    public delegate void SpinksStateChangeEventHandler(SpinksBossEnemyEnum State);
    public event SpinksStateChangeEventHandler Event; 

    public void SendEventMessage(SpinksBossEnemyEnum State)
    {
        Event?.Invoke(State);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<SpinksBossEnemyEnum> StateBlackboardVariable = messageData[0] as BlackboardVariable<SpinksBossEnemyEnum>;
        var State = StateBlackboardVariable != null ? StateBlackboardVariable.Value : default(SpinksBossEnemyEnum);

        Event?.Invoke(State);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        SpinksStateChangeEventHandler del = (State) =>
        {
            BlackboardVariable<SpinksBossEnemyEnum> var0 = vars[0] as BlackboardVariable<SpinksBossEnemyEnum>;
            if(var0 != null)
                var0.Value = State;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as SpinksStateChangeEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as SpinksStateChangeEventHandler;
    }
}

