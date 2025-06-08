using System.Collections.Generic;
using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.LevelEvent
{
    public static class LevelEvents
    {
        public static LevelDataPassEvent levelDataPassEvent = new LevelDataPassEvent();

        // 기본 이동
        public static BasicLevelMoveEvent BasicLevelMoveEvent = new BasicLevelMoveEvent();
        // 방 타입 이동
        public static TypeLevelMoveEvent TypeLevelMoveEvent = new TypeLevelMoveEvent();
        // Grid 이동
        public static PosLevelMoveEvent PosLevelMoveEvent = new PosLevelMoveEvent();

        public static LevelMoveCompleteEvent LevelMoveCompleteEvent = new LevelMoveCompleteEvent();
        public static BossLevelEvent BossLevelEvent = new BossLevelEvent();

        public static InCombatCheckEvent InCombatCheckEvent = new InCombatCheckEvent();
        //public static StageStartEvent StageStartEvent = new StageStartEvent();
        //public static StageEndEvent StageEndEvent = new StageEndEvent();
    }

    public class LevelDataPassEvent : GameEvent
    {
        public Dictionary<Vector2Int, LevelRoom> levelGridDictionary;
    }

    public class BasicLevelMoveEvent : GameEvent
    {
        public DoorDir enterDoorDir;
    }

    public class TypeLevelMoveEvent : GameEvent
    {
        public LevelTypeEnum levelType;
    }

    public class PosLevelMoveEvent : GameEvent
    {
        public Vector2Int pos;
    }

    public class LevelMoveCompleteEvent : GameEvent
    {
        public Vector2Int currentPoint;
    }

    public class BossLevelEvent : GameEvent
    {
        public BTEnemy boss;
    }

    public class InCombatCheckEvent : GameEvent
    {
        public bool isCombat;
    }

    public class StageStartEvent : GameEvent
    {

    }

    public class StageEndEvent : GameEvent
    {

    }
}
