using ObjectPooling;
using Unity.AppUI.UI;
using UnityEngine;
using YH.Enemy;
using YH.Entities;
using YH.EventSystem;

public class WizardEnemy : DefaultEnemy, IDodgeable
{
    private float _lastDodgeTime;
    [SerializeField] private float _dodgeCoolTime = 2f;
    private WizardEnemyAttackCompo _attackCompo;
    [SerializeField] private PoolingItemSO _dodgeEffect;
    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private SoundSO _dodgeSound;

    protected override void Awake()
    {
        base.Awake();
        _attackCompo = GetCompo<WizardEnemyAttackCompo>();
    }

    public void DodgeTrigger()
    {
        if (_attackCompo.isAttack) return;
        if (_lastDodgeTime + _dodgeCoolTime < Time.time)
        {
            _lastDodgeTime = Time.time;
            GetCompo<EnemyMovement>().Dodge();
            var evt = SpawnEvents.EffectSpawn;
            evt.position = transform.position;
            evt.rotation = Quaternion.identity;
            evt.effectItem = _dodgeEffect;
            evt.scale = Vector3.one;
            _spawnChannel.RaiseEvent(evt);


            SoundPlayer sound = PoolManager.Instance.Pop(ObjectType.SoundPlayer) as SoundPlayer;
            sound.PlaySound(_dodgeSound);
            sound.transform.position = transform.position;
        }
    }
}
