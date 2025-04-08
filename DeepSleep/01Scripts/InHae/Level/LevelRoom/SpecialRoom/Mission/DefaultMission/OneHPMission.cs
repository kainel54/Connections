using YH.StatSystem;

public class OneHPMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.clearAction += HandleClearCheck;
        _specialLevelRoom.player.PlayerInput.FireEvent += HandleUseSkillCheck;
        _specialLevelRoom.player.GetCompo<StatCompo>().GetElement("Health").AddModify("OneHP", 0, EModifyMode.Percnet);
        _specialLevelRoom.player.GetCompo<StatCompo>().GetElement("Health").AddModify("OneHP", 1, EModifyMode.Add);
        _specialLevelRoom.StartSpawn();
    }

    private void HandleClearCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        MissionEnd();
        _specialLevelRoom.missionCheckAction.Invoke(true);
        _specialLevelRoom.clearAction -= HandleClearCheck;
    }

    private void HandleUseSkillCheck(bool arg0)
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        MissionEnd();
        _specialLevelRoom.missionCheckAction.Invoke(false);
        _specialLevelRoom.player.PlayerInput.FireEvent -= HandleUseSkillCheck;
    }

    private void MissionEnd()
    {
        _specialLevelRoom.player.GetCompo<StatCompo>().GetElement("Health").RemoveModify("OneHP", EModifyMode.Percnet);
        _specialLevelRoom.player.GetCompo<StatCompo>().GetElement("Health").RemoveModify("OneHP", EModifyMode.Add);
    }
}
