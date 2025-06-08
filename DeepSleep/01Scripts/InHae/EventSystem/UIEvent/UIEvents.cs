using System;
using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.UIEvent
{
    public static class UIEvents
    {
        public static SkillHudEvent SkillHudEvent = new SkillHudEvent();
        
        public static ItemSlotSelectActiveEvent ItemSlotSelectActiveEvent = new ItemSlotSelectActiveEvent();
        public static ItemSlotSelectEvent ItemSlotSelectEvent = new ItemSlotSelectEvent();
    }

    public class SkillHudEvent : GameEvent
    {
        public Skill skill;
        public SkillItemSO SkillItemData;
        public int skillIdx;
    }

    public class ItemSlotSelectActiveEvent : GameEvent
    {
        public bool isActive;
    }
    
    public class ItemSlotSelectEvent : GameEvent
    {
        public RectTransform targetTrm;
    }
}

