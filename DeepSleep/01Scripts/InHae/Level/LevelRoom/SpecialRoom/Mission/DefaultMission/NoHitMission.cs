public class NoHitMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.clearAction += HandleClearCheck;
        _specialLevelRoom.player.GetCompo<HealthCompo>().OnHealthChangedEvent.AddListener(HandleHitCheck);
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

    private void HandleHitCheck(float arg0, float arg1, bool arg2)
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionCheckAction.Invoke(false);
        _specialLevelRoom.player.GetCompo<HealthCompo>().OnHealthChangedEvent.RemoveListener(HandleHitCheck);
    }
}
