namespace YH.EventSystem
{
    public static class PlayerEvents
    {
        public static readonly AddExpEvent AddExp = new AddExpEvent();
        public static readonly FreezePlayerEvent FreezePlayer = new FreezePlayerEvent();
    }

    public class AddExpEvent : GameEvent
    {
        public int exp;
    }

    public class FreezePlayerEvent : GameEvent
    {
    }
}