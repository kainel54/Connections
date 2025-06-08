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
    }

  
    private void OnDestroy()
    {
        _spawnChannel.RemoveListener<PlayerBulletCreate>(HandlePlayerBulletCreate);
        _spawnChannel.RemoveListener<EffectSpawn>(HandleEffectSpawnEvent);
        _spawnChannel.RemoveListener<BallCreate>(HandleBallCreateEvent);    
    }

    private void HandlePlayerBulletCreate(PlayerBulletCreate evt)
    {
        var bullet = PoolManager.Instance.Pop(ProjectileType.Bullet) as Bullet;
        bullet.Fire(evt.position, evt.rotation, evt.payload,evt.owner);
    }
    private void HandleEffectSpawnEvent(EffectSpawn evt)
    {
        var effect = PoolManager.Instance.Pop(evt.effectItem.PoolObj.PoolEnum) as PoolingEffectPlayer;
        effect.PlayEffect(evt.position, evt.rotation,evt.scale,evt.parant);
    }

    private void HandleBallCreateEvent(BallCreate evt)
    {
        EnemyEnergyBall ball = PoolManager.Instance.Pop(ProjectileType.EnemyEnergyBall) as EnemyEnergyBall;
        ball.Fire(evt.position,evt.rotation,evt.payload,evt.owner);
    }
    

}
