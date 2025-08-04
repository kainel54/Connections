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
        public bool isFadeIn; //여기서도 체크하게 해서 out 되었을때 발동 되는 함수 구현
    }
}