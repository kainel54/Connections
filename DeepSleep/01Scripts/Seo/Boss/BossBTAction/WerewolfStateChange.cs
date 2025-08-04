using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/WerewolfStateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "WerewolfStateChange", message: "Werewolf State Assign to [State]", category: "Events", id: "6fd535445c059ed20b5bd4f6da71c494")]
public sealed partial class WerewolfStateChange : EventChannel<WerewolfBossEnum> { }

