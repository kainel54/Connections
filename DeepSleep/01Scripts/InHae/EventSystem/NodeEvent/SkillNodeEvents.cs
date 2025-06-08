using YH.EventSystem;

namespace IH.EventSystem.NodeEvent.SkillNodeEvents
{
    public static class SkillNodeEvents
    {
        public static InitNodeSkillEvent InitNodeSkillEvent = new InitNodeSkillEvent();
        public static SkillStatViewInitEvent SkillStatViewInitEvent = new SkillStatViewInitEvent();
        
        public static EquipPartInfoInitEvent EquipPartInfoInitEvent = new EquipPartInfoInitEvent();
        public static EquipPartInfoEvent EquipPartInfoEvent = new EquipPartInfoEvent();
        
        public static EquipSkillSelectEvent EquipSkillSelectEvent = new EquipSkillSelectEvent();
        
        public static SkillAutoEquipEvent SkillAutoEquipEvent = new SkillAutoEquipEvent();
    }

    public class InitNodeSkillEvent : GameEvent
    {
        public SkillInventoryItem skillInventoryItem;
        public Skill skill;
    }

    public class SkillStatViewInitEvent : GameEvent
    {
        public SkillInventoryItem skillInventoryItem;
        public Skill skill;
    }
    
    public class EquipPartInfoInitEvent : GameEvent
    {
    }

    public class EquipPartInfoEvent : GameEvent
    {
        public SkillInventoryItem skillInventoryItem;
    }

    // 스킬 창에 장착된 스킬을 눌렀을 때 ( 노드 칸이 열릴 떄 ) = true, 노드 칸을 닫았을 때 = flase
    public class EquipSkillSelectEvent : GameEvent
    {
        public bool isSelected;
    }
    
    public class SkillAutoEquipEvent : GameEvent
    {
        public SkillInventoryItem skillInventoryItem;
    }
}