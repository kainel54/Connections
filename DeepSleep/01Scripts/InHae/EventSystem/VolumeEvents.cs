using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.VolumeEvent
{
    public static class VolumeEvents
    {
        public static VignetteSetting VignetteSettingEvent = new VignetteSetting();
        public static VignetteReset VignetteResetEvent = new VignetteReset();
    }

    public class VignetteSetting : GameEvent
    {
        public float lerpTime;
        public float intensity;
        public Color color;
    }

    public class VignetteReset : GameEvent
    {
        
    }
}


