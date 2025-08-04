using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.NodeEvent.PartNodeEvents
{
    public static class PartNodeEvent
    {
        public static AutoEquipPartEvent AutoEquipPartEvent = new AutoEquipPartEvent();
        public static InViewPortNodeParticleEvent InViewPortNodeParticleEvent = new InViewPortNodeParticleEvent();
    }
    
    public class AutoEquipPartEvent : GameEvent
    {
        public PartInventoryItem part;
    }

    public class InViewPortNodeParticleEvent : GameEvent
    {
        public RectTransform nodeRectTrm;
    }
}