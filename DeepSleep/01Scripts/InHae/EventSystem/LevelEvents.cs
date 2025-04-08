using System.Collections.Generic;
using UnityEngine;
using YH.EventSystem;

public static class LevelEvents
{
    public static LevelDataPassEvent levelDataPassEvent = new LevelDataPassEvent();
    public static LevelMoveEvent levelMoveEvent = new LevelMoveEvent();
    public static LevelMoveCompleteEvent LevelMoveCompleteEvent = new LevelMoveCompleteEvent();
    public static BossLevelEvent BossLevelEvent = new BossLevelEvent();
}

public class LevelDataPassEvent : GameEvent
{
    public Dictionary<Vector2Int, LevelRoom> levelGridDictionary;
}

public class LevelMoveEvent : GameEvent
{
    public DoorDir enterDoorDir;
}

public class LevelMoveCompleteEvent : GameEvent
{
    public Vector2Int currentPoint;
}

public class BossLevelEvent : GameEvent
{
    public BTEnemy boss;
}