public class OnlyNormalAttackMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.clearAction += HandleClearCheck;
        _specialLevelRoom.player.PlayerInput.UseSkillEvent += HandleUseSkillCheck;
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

    private void HandleUseSkillCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionCheckAction.Invoke(false);
        _specialLevelRoom.player.PlayerInput.UseSkillEvent -= HandleUseSkillCheck;
    }
}
