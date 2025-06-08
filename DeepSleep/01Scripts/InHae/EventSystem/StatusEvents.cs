using YH.Entities;
using YH.EventSystem;

namespace IH.EventSystem.StatusEvent
{
    public static class StatusEvents
    {
        public static AddStatusEvent AddStatusEvent = new AddStatusEvent();
        public static RemoveStatusEvent RemoveStatusEvent = new RemoveStatusEvent();
        public static AddTimeStatusEvent AddTimeStatusEvent = new AddTimeStatusEvent();
        public static RemoveAllStatusEvent RemoveAllStatusEvent = new RemoveAllStatusEvent();
        
        // 플레이어가 스테이터스를 추가 '당했을' 때 이벤트들 ( UI 추가 등 )
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
}

