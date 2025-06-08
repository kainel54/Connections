using IH.EventSystem.MissionEvent;

public class OnlyNormalAttackMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();
        
        _missionEventChannel.AddListener<OnlyNormalAttackMissionFailCheckEvent>(HandleFailCheck);

        var onlyNormalAttackMissionCheckEvt = MissionEvents.OnlyNormalAttackMissionStartEvent;
        onlyNormalAttackMissionCheckEvt.isStart = true;
        _missionEventChannel.RaiseEvent(onlyNormalAttackMissionCheckEvt);

        _specialLevelRoom.missionClearCheckAction += HandleClearCheck;
        _specialLevelRoom.StartSpawn();
    }

    private void HandleClearCheck()
    {
        if (_missionEnd)
            return;

        _specialLevelRoom.missionActiveAction.Invoke(true);
        _specialLevelRoom.missionClearCheckAction -= HandleClearCheck;
        EndProcess();
    }
    
    private void HandleFailCheck(OnlyNormalAttackMissionFailCheckEvent evt)
    {
        if (_missionEnd)
            return;

        _specialLevelRoom.missionActiveAction.Invoke(false);
        EndProcess();
    }

    private void EndProcess()
    {
        _missionEnd = true;

        var onlyNormalAttackMissionCheckEvt = MissionEvents.OnlyNormalAttackMissionStartEvent;
        onlyNormalAttackMissionCheckEvt.isStart = false;
        _missionEventChannel.RaiseEvent(onlyNormalAttackMissionCheckEvt);
        
        _missionEventChannel.RemoveListener<OnlyNormalAttackMissionFailCheckEvent>(HandleFailCheck);
    }
}
