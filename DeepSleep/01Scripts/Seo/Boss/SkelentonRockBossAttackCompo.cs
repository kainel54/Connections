using ObjectPooling;
using UnityEngine;
using YH.Combat;
using YH.Enemy;
using YH.EventSystem;

public class SkelentonRockBossAttackCompo : EnemyAttackCompo
{
    [SerializeField] private EnemyDamageCaster _damageCaster;
    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private float _displayLifeTime;
    [SerializeField] private float _dashHeight = 7;
    [SerializeField] private float _dashWidth = 2;

    private OverlapCircleDamageCaster _currentCircleCaster;

    private Vector3 _targetPos;


   
}
