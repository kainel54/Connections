using YH.EventSystem;
using UnityEngine;

namespace IH.EventSystem.MissionEvent
{
    public static class MissionEvents
    {
        public static MissionInitEvent MissionInitEvent = new MissionInitEvent();
        public static MissionCheckEvent MissionCheckEvent = new MissionCheckEvent();
        public static MissionEtcTextEvent MissionEtcTextEvent = new MissionEtcTextEvent();
        public static OnlyNormalAttackMissionStartEvent OnlyNormalAttackMissionStartEvent = new OnlyNormalAttackMissionStartEvent();
        public static OnlyNormalAttackMissionFailCheckEvent OnlyNormalAttackMissionFailCheckEvent = new OnlyNormalAttackMissionFailCheckEvent();
    }

    public class MissionInitEvent : GameEvent
    {
        public string missionDescription;
    }

    public class MissionEtcTextEvent : GameEvent
    {
        public bool isActive;
        public string text;
        public Color color;
    }

    public class MissionCheckEvent: GameEvent
    {
        public bool missionCheck;
    }

    public class OnlyNormalAttackMissionStartEvent : GameEvent
    {
        public bool isStart;
    }
    
    public class OnlyNormalAttackMissionFailCheckEvent : GameEvent
    {
    }
}
