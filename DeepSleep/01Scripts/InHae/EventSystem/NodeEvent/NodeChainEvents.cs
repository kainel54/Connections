using YH.EventSystem;

namespace IH.EventSystem.NodeEvent.NodeChainEvent
{
    public static class NodeChainEvents
    {
        public static ChainModeChangeEvent ChainModeChangeEvent = new ChainModeChangeEvent();
        public static ChainPartSelectEvent ChainPartSelectEvent = new ChainPartSelectEvent();
        public static ChainPartSelectCompleteEvent ChainPartSelectCompleteEvent = new ChainPartSelectCompleteEvent();
    }
    
    public class ChainModeChangeEvent : GameEvent
    {
        public bool isActive;
    }

    public class ChainPartSelectEvent : GameEvent
    {
        public PartItemSO partItemSO;
    }

    public class ChainPartSelectCompleteEvent : GameEvent
    {
        
    }
}
