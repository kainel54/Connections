using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.SoundEvent
{
    public static class SoundEvents
    {
        public static PlaySFXEvent PlaySfxEvent = new PlaySFXEvent();
        public static PlayBGMEvent PlayBGMEvent = new PlayBGMEvent();
        public static StopBGMEvent StopBGMEvent = new StopBGMEvent();
        public static PlayLoopSFXEvent PlayLoopSFXEvent = new PlayLoopSFXEvent();
        public static StopLoopSFXEvent StopLoopSFXEvent = new StopLoopSFXEvent();
    }

    public class PlaySFXEvent : GameEvent
    {
        public SoundSO clipData;
        public Vector3 position;
    }

    public class PlayLoopSFXEvent : GameEvent
    {
        public SoundSO clipData;
        public Vector3 position;
    }

    public class StopLoopSFXEvent : GameEvent
    {
    }


    public class PlayBGMEvent : GameEvent
    {
        public SoundSO clipData;
    }

    public class StopBGMEvent : GameEvent
    {
        public SoundSO clipData;
    }
}
