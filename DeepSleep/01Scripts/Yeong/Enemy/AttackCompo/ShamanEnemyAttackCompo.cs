using ObjectPooling;
using System.Collections.Generic;
using IH.EventSystem.StatusEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

namespace YH.Enemy
{
    public class ShamanEnemyAttackCompo : EnemyAttackCompo
    {
        [SerializeField] private GameEventChannelSO _spawnChannel;
        [SerializeField] private GameEventChannelSO _statusChannel;
        [SerializeField] private PoolingItemSO _sorceryEffectItem;
        [SerializeField] private List<StatusEnum> _statusEnums;
        private Player _player;

        private void Start()
        {
            _player = _enemy.player.GetComponent<Player>();
        }

        public void Sorcery()
        {
            var evt = StatusEvents.AddTimeStatusEvent;
            int randomIdx = Random.Range(0, _statusEnums.Count);
            evt.entity = _player;
            evt.status = _statusEnums[randomIdx];
            evt.time = 15f;
            _statusChannel.RaiseEvent(evt);

            var buffEffect = PoolManager.Instance.Pop(EffectPoolingType.DebuffEffectCircle) as PoolingDefaultEffectPlayer;
            buffEffect.SetDuration(15f);
            buffEffect.PlayEffect(_player.transform.position, Quaternion.identity, new Vector3(0.5f,0.5f,0.5f), _player.transform);

        }

        public void CreateEffect()
        {
            var evt = SpawnEvents.EffectSpawn;
            evt.effectItem = _sorceryEffectItem;
            evt.position = _enemy.transform.position;
            evt.rotation = Quaternion.LookRotation(_enemy.transform.forward);
            evt.scale = Vector3.one;
            _spawnChannel.RaiseEvent(evt);
        }
    }
}

