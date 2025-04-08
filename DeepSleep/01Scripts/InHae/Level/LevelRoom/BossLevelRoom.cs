using UnityEngine;
using YH.EventSystem;

public class BossLevelRoom : LevelRoom
{
    [SerializeField] private BTEnemy _boss;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameEventChannelSO _levelEvent;
    [SerializeField] private DefaultRoomChest _chest;
    [SerializeField] private ClearPortal _portal;

    private BTEnemy _currentBoss;
    public override void EnterEvent()
    {
        if(isClear)
            return;

        _currentBoss = Instantiate(_boss, _spawnPoint.position, Quaternion.identity, transform);
        var evt = LevelEvents.BossLevelEvent;
        evt.boss = _currentBoss;
        
        _currentBoss.GetCompo<HealthCompo>().OnDieEvent.AddListener(HandleDeadEvent);
        
        _levelEvent.RaiseEvent(evt);
    }

    private void HandleDeadEvent()
    {
        _currentBoss.GetCompo<HealthCompo>().OnDieEvent.RemoveListener(HandleDeadEvent);
        
        ClearPortal closePortal = Instantiate(_portal, _spawnPoint.position, Quaternion.identity, transform);
        closePortal.Init();
    }

    public override void LevelClear()
    {
        base.LevelClear();
        _chest.Open();
    }
}
