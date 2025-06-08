using YH.StatSystem;

public class OneHPMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.missionClearCheckAction += HandleClearCheck;
        _specialLevelRoom.player.PlayerInput.AttackEvent += HandleUseSkillCheck;
        _specialLevelRoom.player.GetCompo<EntityStat>().GetElement("Health").AddModify("OneHP", 0, EModifyMode.Percnet);
        _specialLevelRoom.player.GetCompo<EntityStat>().GetElement("Health").AddModify("OneHP", 1, EModifyMode.Add);
        
        _specialLevelRoom.StartSpawn();
    }

    private void HandleClearCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        MissionEnd();
        _specialLevelRoom.missionActiveAction.Invoke(true);
        _specialLevelRoom.missionClearCheckAction -= HandleClearCheck;
    }

    private void HandleUseSkillCheck(bool arg0)
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        MissionEnd();
        _specialLevelRoom.missionActiveAction.Invoke(false);
        _specialLevelRoom.player.PlayerInput.AttackEvent -= HandleUseSkillCheck;
    }

    private void MissionEnd()
    {
        _specialLevelRoom.player.GetCompo<EntityStat>().GetElement("Health").RemoveModify("OneHP", EModifyMode.Percnet);
        _specialLevelRoom.player.GetCompo<EntityStat>().GetElement("Health").RemoveModify("OneHP", EModifyMode.Add);
    }
}
