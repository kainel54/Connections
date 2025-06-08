public class NoHitMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.missionClearCheckAction += HandleClearCheck;
        
        _specialLevelRoom.player.GetCompo<EntityHealth>().OnHitEvent.AddListener(HandleHitCheck);
        _specialLevelRoom.StartSpawn();
    }

    private void HandleClearCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionActiveAction.Invoke(true);
        _specialLevelRoom.missionClearCheckAction -= HandleClearCheck;
    }

    private void HandleHitCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionActiveAction.Invoke(false);
        _specialLevelRoom.player.GetCompo<EntityHealth>().OnHitEvent.RemoveListener(HandleHitCheck);
    }
}
