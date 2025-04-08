using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/AnimParamChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "AnimParamChange", message: "set to [param]", category: "Events", id: "4e7897057219213852b20190b8131b29")]
public partial class AnimParamChange : EventChannelBase
{
    public delegate void AnimParamChangeEventHandler(string param);
    public event AnimParamChangeEventHandler Event; 

    public void SendEventMessage(string param)
    {
        Event?.Invoke(param);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<string> paramBlackboardVariable = messageData[0] as BlackboardVariable<string>;
        var param = paramBlackboardVariable != null ? paramBlackboardVariable.Value : default(string);

        Event?.Invoke(param);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        AnimParamChangeEventHandler del = (param) =>
        {
            BlackboardVariable<string> var0 = vars[0] as BlackboardVariable<string>;
            if(var0 != null)
                var0.Value = param;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as AnimParamChangeEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as AnimParamChangeEventHandler;
    }
}

