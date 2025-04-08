using System;
using System.Collections;
using UnityEngine;

public class TimeAttackMission : SpecialLevelMission
{
    private event Action OnClearEvent;
    [SerializeField] private float duration;

    public override void Init()
    {
        base.Init();

        _specialLevelRoom.clearAction += HandleClearCheck;
        _specialLevelRoom.player.GetCompo<HealthCompo>().OnDieEvent.AddListener(HandleTimeOutCheck);
        OnClearEvent += HandleTimeOutCheck;

        StartCoroutine(Timer(duration));

        _specialLevelRoom.StartSpawn();
    }

    private void HandleClearCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionCheckAction.Invoke(true);
        _specialLevelRoom.clearAction -= HandleClearCheck;
    }

    private void HandleTimeOutCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionCheckAction.Invoke(false);
        _specialLevelRoom.player.GetCompo<HealthCompo>().OnDieEvent.AddListener(HandleTimeOutCheck);
        OnClearEvent -= HandleTimeOutCheck;
    }

    private IEnumerator Timer(float duration)
    {
        float timer = duration;

        while(duration > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        OnClearEvent?.Invoke();
    }
}
