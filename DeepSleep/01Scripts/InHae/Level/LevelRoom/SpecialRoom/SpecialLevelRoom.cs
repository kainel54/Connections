using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.LevelEvent;
using IH.EventSystem.MissionEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Players;
using Random = UnityEngine.Random;

public class SpecialLevelRoom : LevelRoom
{
    [SerializeField] private GameEventChannelSO _levelEventChannel;
    [SerializeField] private GameEventChannelSO _missionEventChannel;
    
    [SerializeField] private Transform _missionParent;
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    
    [SerializeField] private DefaultRoomChest _chest;

    [HideInInspector] public Player player;

    public Action<bool> missionActiveAction;
    public Action missionClearCheckAction;
    private bool _missionClear = true;

    private List<SpecialLevelMission> _missions = new List<SpecialLevelMission>();
    private SpecialLevelMission _selectMission;
    
    private Spawner _spawner;

    protected override void Awake()
    {
        base.Awake();
        _spawner = GetComponent<Spawner>();
        
        _playerManagerSo.SetUpPlayerEvent += HandleSettingPlayer;

        _missions = _missionParent.GetComponentsInChildren<SpecialLevelMission>().ToList();
        missionActiveAction += HandleMissionCheck;
    }

    private void OnDestroy()
    {
        _playerManagerSo.SetUpPlayerEvent -= HandleSettingPlayer;
    }

    private void HandleSettingPlayer()
    {
        player = _playerManagerSo.Player;
    }
    
    private void HandleMissionCheck(bool isSuccess)
    {
        _missionClear = isSuccess;

        var evt = MissionEvents.MissionCheckEvent;
        evt.missionCheck = _missionClear;
        _missionEventChannel.RaiseEvent(evt);
    }

    public override void EnterEvent()
    {
        if(isClear)
            return;
        
        var inCombatEvt = LevelEvents.InCombatCheckEvent;
        inCombatEvt.isCombat = true;
        _levelEventChannel.RaiseEvent(inCombatEvt);

        _selectMission = _missions[Random.Range(0, _missions.Count)];
        _selectMission.SetRoom(this);
        _selectMission.Init();
    }

    public void StartSpawn()
    {
        _spawner.SetWave();
        StartCoroutine(SpawnDelay(1));
    }

    private IEnumerator SpawnDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _spawner.Spawn();
    }

    public override void LevelClear()
    {
        if (isClear) 
            return;
        base.LevelClear();
        
        var inCombatEvt = LevelEvents.InCombatCheckEvent;
        inCombatEvt.isCombat = false;
        _levelEventChannel.RaiseEvent(inCombatEvt);

        if (_missionClear)
        {
            missionClearCheckAction?.Invoke();
            _chest.Open();
        }
    }
}
