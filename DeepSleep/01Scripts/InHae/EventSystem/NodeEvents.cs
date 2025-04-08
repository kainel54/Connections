using YH.EventSystem;

namespace IH.EventSystem
{
    public static class NodeEvents
    {
        public static InitNodeSkillEvent InitNodeSkillEvent = new InitNodeSkillEvent();
        public static NodeUpgradeEvent NodeUpgradeEvent = new NodeUpgradeEvent();
        public static SkillStatViewInitEvent SkillStatViewInitEvent = new SkillStatViewInitEvent();
        public static ChainModeChangeEvent ChainModeChangeEvent = new ChainModeChangeEvent();
        public static ChainPartSelectEvent ChainPartSelectEvent = new ChainPartSelectEvent();
        public static ChainPartSelectCompleteEvent ChainPartSelectCompleteEvent = new ChainPartSelectCompleteEvent();
    }

    public class InitNodeSkillEvent : GameEvent
    {
        public SkillInventoryItem skillInventoryItem;
        public Skill skill;
    }
    
    public class NodeUpgradeEvent : GameEvent
    {
        public SkillInventoryItem skillInventoryItem;
        public int count;
    }

    public class SkillStatViewInitEvent : GameEvent
    {
        public SkillInventoryItem skillInventoryItem;
        public Skill skill;
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