using YH.EventSystem;

namespace IH.EventSystem.NodeEvent.PartNodeEvents
{
    public static class PartNodeEvent
    {
        public static AutoEquipPartEvent AutoEquipPartEvent = new AutoEquipPartEvent();
    }
    
    public class AutoEquipPartEvent : GameEvent
    {
        public PartInventoryItem part;
    }
}