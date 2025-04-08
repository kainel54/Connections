using YH.EventSystem;

public static class MissionEvents
{
    public static MissionInitEvent MissionInitEvent = new MissionInitEvent();
    public static MissionCheckEvent MissionCheckEvent = new MissionCheckEvent();
}

public class MissionInitEvent : GameEvent
{
    public string missionDescription;
}

public class MissionCheckEvent: GameEvent
{
    public bool missionCheck;
}