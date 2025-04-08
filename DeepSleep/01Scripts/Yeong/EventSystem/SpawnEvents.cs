using ObjectPooling;
using UnityEngine;
using YH.Entities;
using YH.EventSystem;

public static class SpawnEvents
{
    public static PlayerBulletCreate PlayerBulletCreate = new PlayerBulletCreate();
    public static EffectSpawn EffectSpawn = new EffectSpawn();
    public static BallCreate BallCreate = new BallCreate();
    public static SlashCreate SlashCreate = new SlashCreate();
}

public class PlayerBulletCreate : GameEvent
{
    public Vector3 position;
    public Quaternion rotation;
    public BulletPayload payload;
    public Entity owner;
}

public class EffectSpawn : GameEvent
{
    public PoolingItemSO effectItem;
    public Vector3 position;
    public Quaternion rotation;
}

public class BallCreate : GameEvent
{
    public Vector3 position;
    public Quaternion rotation;
    public BulletPayload payload;
    public Entity owner;
}

public class SlashCreate : GameEvent
{
    public Vector3 position;
    public Quaternion rotation;
    public BulletPayload payload;
    public Entity owner;
}