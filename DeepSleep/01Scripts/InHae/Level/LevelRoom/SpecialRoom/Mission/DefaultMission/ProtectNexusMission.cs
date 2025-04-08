using UnityEngine;

public class ProtectNexusMission : SpecialLevelMission
{
    public override void Init()
    {
        base.Init();

        _specialLevelRoom.clearAction += HandleClearCheck;
        _specialLevelRoom.player.GetCompo<HealthCompo>().OnDieEvent.AddListener(HandleTimeOutCheck);

        // ³Ø¼­½º ½ºÆù

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
    }
}
