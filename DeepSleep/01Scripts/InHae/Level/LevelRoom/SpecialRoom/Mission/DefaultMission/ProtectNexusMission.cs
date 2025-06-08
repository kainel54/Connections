using UnityEngine;

public class ProtectNexusMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.missionClearCheckAction += HandleClearCheck;
        _specialLevelRoom.player.GetCompo<EntityHealth>().OnDieEvent.AddListener(HandleTimeOutCheck);

        // �ؼ��� ����

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

    private void HandleTimeOutCheck()
    {
        if (_missionEnd)
            return;

        _missionEnd = true;
        _specialLevelRoom.missionActiveAction.Invoke(false);
        _specialLevelRoom.player.GetCompo<EntityHealth>().OnDieEvent.AddListener(HandleTimeOutCheck);
    }
}
