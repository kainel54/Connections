public class OnlySkillMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.missionClearCheckAction += HandleClearCheck;
        _specialLevelRoom.player.PlayerInput.AttackEvent += HandleUseSkillCheck;
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

    private void HandleUseSkillCheck(bool arg0)
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionActiveAction.Invoke(false);
        _specialLevelRoom.player.PlayerInput.AttackEvent -= HandleUseSkillCheck;
    }
}
