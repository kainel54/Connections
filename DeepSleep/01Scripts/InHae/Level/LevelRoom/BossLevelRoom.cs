using IH.EventSystem.LevelEvent;
using UnityEngine;
using YH.EventSystem;

public class BossLevelRoom : LevelRoom
{
    [SerializeField] private GameEventChannelSO _endStageEventChannel;
    [SerializeField] private GameEventChannelSO _levelEventChannel;
    [SerializeField] private BTEnemy _boss;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private DefaultRoomChest _chest;
    [SerializeField] private ClearPortal _portal;

    private BTEnemy _currentBoss;
    private BossRoomEnterCutScene _currnetCutSceneObj;
    public override void EnterEvent()
    {
        if (isClear)
            return;

        if (HasCutScene)
        {
            _currnetCutSceneObj = Instantiate(_cutSceneObject, _spawnPoint.position, Quaternion.identity, transform);
            _currnetCutSceneObj.cutSceneEnded += HandleCutSceneEnded;
        }
        else
        {
            SpawnAndSettingBossRoom();
        }
    }

    private void HandleCutSceneEnded()
    {
        _currnetCutSceneObj.cutSceneEnded -= HandleCutSceneEnded;
        SpawnAndSettingBossRoom();
    }

    private void SpawnAndSettingBossRoom()
    {
        var inCombatEvt = LevelEvents.InCombatCheckEvent;
        inCombatEvt.isCombat = true;
        _levelEventChannel.RaiseEvent(inCombatEvt);

        _currentBoss = Instantiate(_boss, _spawnPoint.position, Quaternion.identity, transform);
        _currentBoss.GetCompo<EntityHealth>().OnDieEvent.AddListener(HandleDeadEvent);

        var bossLevelEvent = LevelEvents.BossLevelEvent;
        bossLevelEvent.boss = _currentBoss;
        _levelEventChannel.RaiseEvent(bossLevelEvent);
    }

    private void HandleDeadEvent()
    {
        _currentBoss.GetCompo<EntityHealth>().OnDieEvent.RemoveListener(HandleDeadEvent);

        ClearPortal closePortal = Instantiate(_portal, _spawnPoint.position, Quaternion.identity, transform);
        closePortal.Init();
    }

    public override void LevelClear()
    {
        if (isClear)
            return;

        base.LevelClear();

        var evt = LevelEvents.StageEndEvent;
        _endStageEventChannel.RaiseEvent(evt);

        var inCombatEvt = LevelEvents.InCombatCheckEvent;
        inCombatEvt.isCombat = false;
        _levelEventChannel.RaiseEvent(inCombatEvt);
    }
}
