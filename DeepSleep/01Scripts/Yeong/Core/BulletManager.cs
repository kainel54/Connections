using ObjectPooling;
using System;
using UnityEngine;
using YH.EventSystem;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _spawnChannel;


    private void Start()
    {
        _spawnChannel.AddListener<PlayerBulletCreate>(HandlePlayerBulletCreate);
        _spawnChannel.AddListener<EffectSpawn>(HandleEffectSpawnEvent);
        _spawnChannel.AddListener<BallCreate>(HandleBallCreateEvent);
        _spawnChannel.AddListener<SlashCreate>(HandleSlashCreateEvent);
    }

  
    private void OnDestroy()
    {
        _spawnChannel.Clear();
    }

    private void HandlePlayerBulletCreate(PlayerBulletCreate evt)
    {
        var bullet = PoolManager.Instance.Pop(PoolingType.Bullet) as Bullet;
        bullet.Fire(evt.position, evt.rotation, evt.payload,evt.owner);
    }
    private void HandleEffectSpawnEvent(EffectSpawn evt)
    {
        var effect = PoolManager.Instance.Pop(evt.effectItem.poolingType) as PoolingEffectPlayer;
        effect.PlayEffect(evt.position, evt.rotation);
    }

    private void HandleBallCreateEvent(BallCreate evt)
    {
        EnemyEnergyBall ball = PoolManager.Instance.Pop(PoolingType.EnemyEnergyBall) as EnemyEnergyBall;
        ball.Fire(evt.position,evt.rotation,evt.payload,evt.owner);
    }
    private void HandleSlashCreateEvent(SlashCreate evt)
    {
        GroundSlash slash = PoolManager.Instance.Pop(PoolingType.GroundSlash) as GroundSlash;
        slash.Fire(evt.position, evt.rotation, evt.payload, evt.owner);
    }

}
