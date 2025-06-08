using IH.EventSystem.LevelEvent;
using System.Collections;
using UnityEngine;
using YH.EventSystem;

public class DefaultLevelRoom : LevelRoom
{
    [SerializeField] private GameEventChannelSO _levelEventChannel;
    [SerializeField] private DefaultRoomChest _chest;
    private Spawner _spawner;

    protected override void Awake()
    {
        base.Awake();
        _spawner = GetComponent<Spawner>();
    }

    public override void EnterEvent()
    {
        if (isClear)
            return;

        var inCombatEvt = LevelEvents.InCombatCheckEvent;
        inCombatEvt.isCombat = true;
        _levelEventChannel.RaiseEvent(inCombatEvt);

        StartSpawn();
    }

    private void StartSpawn()
    {
        _spawner.SetWave();
        StartCoroutine(SpawnDelay(1));
    }

    private IEnumerator SpawnDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _spawner.Spawn();
    }

    public override void LevelClear()
    {
        if (isClear)
            return;

        base.LevelClear();

        var inCombatEvt = LevelEvents.InCombatCheckEvent;
        inCombatEvt.isCombat = false;
        _levelEventChannel.RaiseEvent(inCombatEvt);

        _chest.Open();
    }
}
