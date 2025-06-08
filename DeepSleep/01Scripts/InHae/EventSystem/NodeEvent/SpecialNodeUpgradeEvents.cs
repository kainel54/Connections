using System.Collections.Generic;
using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.NodeEvent.SpecialPartNodeEvent
{
    public static class SpecialNodeUpgradeEvents
    {
        public static NodeParentInitEvent NodeParentInitEvent = new NodeParentInitEvent();
        
        public static UpgradeNodeSelectEvent UpgradeNodeSelectEvent = new UpgradeNodeSelectEvent();
        public static UpgradeNodeInitEvent UpgradeNodeInitEvent = new UpgradeNodeInitEvent();
        
        public static UpgradeSkillSelectEvent UpgradeSkillSelectEvent = new UpgradeSkillSelectEvent();
        public static UpgradeSkillInitEvent UpgradeSkillInitEvent = new UpgradeSkillInitEvent();
        public static UpgradeSkillReLoadEvent UpgradeSkillReLoadEvent = new UpgradeSkillReLoadEvent();
        public static UpgradeSkillSelectLockEvent UpgradeSkillSelectLockEvent = new UpgradeSkillSelectLockEvent();

        public static EquipSkillSlotInitEvent EquipSkillSlotInitEvent = new EquipSkillSlotInitEvent();
        public static UpgradeSlotSelectImageEvent UpgradeSlotSelectImageEvent = new UpgradeSlotSelectImageEvent();
    }

    public class NodeParentInitEvent : GameEvent
    {
        public Transform parent;
    }
    
    public class UpgradeNodeSelectEvent : GameEvent
    {
        public bool isSelected;
        public SpecialUpgradePartNode selectNode;
    }
    
    public class UpgradeSkillSelectEvent : GameEvent
    {
        public SkillInventoryItem item;
    }

    public class UpgradeSkillInitEvent : GameEvent
    {
    }
    
    public class UpgradeSkillReLoadEvent : GameEvent
    {
    }
    
    public class UpgradeNodeInitEvent : GameEvent
    {
    }
    
    public class EquipSkillSlotInitEvent : GameEvent
    {
        public List<SkillEquipSlot> slots;
    }
    
    public class UpgradeSlotSelectImageEvent : GameEvent
    {
        public RectTransform targetTrm;
    }
    
    public class UpgradeSkillSelectLockEvent : GameEvent
    {
        public bool isLocked;
    }
}