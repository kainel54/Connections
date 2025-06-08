using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.InteractEvent
{
    public static class InteractEvents
    {
        public static SkillInteractDescriptionPanelEvent SkillInteractDescriptionPanelEvent = new SkillInteractDescriptionPanelEvent();
        public static DefaultInteractDescriptionEvent DefaultInteractDescriptionEvent = new DefaultInteractDescriptionEvent();
    }
    
    public class DefaultInteractDescriptionEvent: GameEvent
    {
        public bool isPanelActive;
        public string title;
        public string description;
        public float yOffset;
        public Vector3 position;
    }

    public class SkillInteractDescriptionPanelEvent: GameEvent
    {
        public bool isPanelActive;
        public ItemDataSO itemDataSo;
        public Vector3 position;
    }
}

