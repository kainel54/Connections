using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.NodeEvent.SpecialPartNodeEvent
{
    public static class SpecialPartNodeEvents
    {
        public static SetSpecialModeEvent SetSpecialModeEvent = new SetSpecialModeEvent();
        public static ChangeSpecialModeEvent ChangeSpecialModeEvent = new ChangeSpecialModeEvent();
        public static AutoEquipAbilityEvent AutoEquipAbilityEvent = new AutoEquipAbilityEvent();
    }

    public class SetSpecialModeEvent : GameEvent
    {
        public bool isSpecialMode;
    }

    public class ChangeSpecialModeEvent : GameEvent
    {
        
    }
    
    public class AutoEquipAbilityEvent : GameEvent
    {
        public NodeAbilityInventoryItem ability;
    }}