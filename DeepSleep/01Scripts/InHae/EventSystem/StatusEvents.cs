using YH.Entities;
using YH.EventSystem;

public static class StatusEvents
{
    public static AddStatusEvent AddStatusEvent = new AddStatusEvent();
    public static RemoveStatusEvent RemoveStatusEvent = new RemoveStatusEvent();
    public static AddTimeStatusEvent AddTimeStatusEvent = new AddTimeStatusEvent();
    public static RemoveAllStatusEvent RemoveAllStatusEvent = new RemoveAllStatusEvent();
    
    public static PlayerIsAddedStatusEvent PlayerIsAddedStatusEvent = new PlayerIsAddedStatusEvent();
    public static PlayerIsAddedTimeStatusEvent PlayerIsAddedTimeStatusEvent = new PlayerIsAddedTimeStatusEvent();
    public static PlayerIsRemovedStatusEvent PlayerIsRemovedStatusEvent = new PlayerIsRemovedStatusEvent();
}

public class AddStatusEvent : GameEvent
{
    public Entity entity;
    public StatusEnum status;
}

public class AddTimeStatusEvent : GameEvent
{
    public Entity entity;
    public StatusEnum status;
    public float time;
}

public class RemoveStatusEvent : GameEvent
{
    public Entity entity;
    public StatusEnum status;
}

public class RemoveAllStatusEvent : GameEvent
{
    public Entity entity;
}


public class PlayerIsAddedStatusEvent : GameEvent
{
    public StatusEnum type;
    public StatusStat status;
}

public class PlayerIsAddedTimeStatusEvent : GameEvent
{
    public StatusEnum type;
    public StatusStat status;
}

public class PlayerIsRemovedStatusEvent : GameEvent
{
    public StatusEnum type;
    public StatusStat status;
}

