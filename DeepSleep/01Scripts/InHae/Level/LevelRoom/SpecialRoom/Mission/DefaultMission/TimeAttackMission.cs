using System;
using System.Collections;
using IH.EventSystem.MissionEvent;
using UnityEngine;

public class TimeAttackMission : SpecialLevelMission
{
    private event Action OnClearEvent;
    [SerializeField] private float duration;
    
    private Coroutine _coroutine;

    public override void Init()
    {
        base.Init();

        _specialLevelRoom.missionClearCheckAction += HandleClearCheck;
        _specialLevelRoom.player.GetCompo<EntityHealth>().OnDieEvent.AddListener(HandleTimeOutCheck);
        OnClearEvent += HandleTimeOutCheck;

        _coroutine = StartCoroutine(Timer(duration));

        _specialLevelRoom.StartSpawn();
    }

    private void HandleClearCheck()
    {
        if (_missionEnd)
            return;
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        var timerTextEvent = MissionEvents.MissionEtcTextEvent;
        timerTextEvent.isActive = false;
        _missionEventChannel.RaiseEvent(timerTextEvent);

        _missionEnd = true;
        _specialLevelRoom.missionActiveAction.Invoke(true);
        _specialLevelRoom.missionClearCheckAction -= HandleClearCheck;
    }

    private void HandleTimeOutCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionActiveAction.Invoke(false);
        _specialLevelRoom.player.GetCompo<EntityHealth>().OnDieEvent.AddListener(HandleTimeOutCheck);
        
        var timerTextEvent = MissionEvents.MissionEtcTextEvent;
        timerTextEvent.isActive = false;
        _missionEventChannel.RaiseEvent(timerTextEvent);
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        OnClearEvent -= HandleTimeOutCheck;
    }

    private IEnumerator Timer(float duration)
    {
        float timer = duration;

        var timerTextEvent = MissionEvents.MissionEtcTextEvent;
        timerTextEvent.isActive = true;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            timerTextEvent.text = $"남은 시간: {timer:0}초";
            timerTextEvent.color = timer <= 10 ? Color.red : Color.white;
            _missionEventChannel.RaiseEvent(timerTextEvent);
            
            yield return null;
        }

        timerTextEvent.isActive = false;
        _missionEventChannel.RaiseEvent(timerTextEvent);
        OnClearEvent?.Invoke();
    }
}
