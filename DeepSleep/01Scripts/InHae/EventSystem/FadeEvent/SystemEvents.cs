using YH.EventSystem;

namespace IH.EventSystem.SystemEvent
{
    public static class SystemEvents
    {
        public static FadeScreenEvent FadeScreenEvent = new FadeScreenEvent();
        public static FadeComplete FadeComplete = new FadeComplete();
        public static FirstFadeSetting FirstFadeSetting = new FirstFadeSetting();
    }
    
    public class FirstFadeSetting : GameEvent
    {
        
    }
    
    public class FadeScreenEvent : GameEvent
    {
        public bool isCircle;
        // true 가 밝아지는 거
        public bool isFadeIn;
        public float fadeDuration;
    }

    public class FadeComplete : GameEvent
    {
        
    }
}