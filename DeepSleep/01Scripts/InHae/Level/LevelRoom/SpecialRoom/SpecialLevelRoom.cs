using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;
using YH.Players;
using Random = UnityEngine.Random;

public class SpecialLevelRoom : LevelRoom
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    [SerializeField] private GameEventChannelSO _missionEventChannel;
    
    [SerializeField] private DefaultRoomChest _chest;

    [HideInInspector] public Player player;

    public Action<bool> missionCheckAction;
    public Action clearAction;
    private bool _missionClear = true;

    private List<SpecialLevelMission> _missions = new List<SpecialLevelMission>();
    private SpecialLevelMission _selectMission;
    private Spawner _spawner;

    private void Start()
    {
        _playerManagerSo.SetUpPlayerEvent += HandleSettingPlayer;

        _missions = GetComponentsInChildren<SpecialLevelMission>().ToList();
        missionCheckAction += HandleMissionCheck;
        _spawner = GetComponent<Spawner>();
        _spawner.levelClearEvent += HandleLevelClearEvent;
    }

    

    private void OnDestroy()
    {
        _playerManagerSo.SetUpPlayerEvent -= HandleSettingPlayer;
        _spawner.levelClearEvent -= HandleLevelClearEvent;
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

        _selectMission = _missions[Random.Range(0, _missions.Count)];
        _selectMission.SetRoom(this);
        _selectMission.Init();
    }

    public void StartSpawn()
    {
        _spawner.Spawn();
    }

    private void HandleLevelClearEvent()
    {
        if (isClear)
            return;
        LevelClear();
    }
    public override void LevelClear()
    {
        base.LevelClear();

        if (_missionClear)
        {
            clearAction?.Invoke();
            _chest.Open();
        }
    }

    
}
