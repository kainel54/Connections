using IH.EventSystem.LevelEvent;
using System.Collections;
using UnityEngine;
using YH.EventSystem;

public class DefaultLevelRoom : LevelRoom
{
    [SerializeField] private GameEventChannelSO _endStageEventChannel;
    [SerializeField] private GameEventChannelSO _levelEventChannel;
    [SerializeField] private DefaultRoomChest _chest;
    [SerializeField] private TotemManager _totem;
    private Spawner _spawner;
    
    private Coroutine _coroutine;

    protected override void Awake()
    {
        base.Awake();
        _spawner = GetComponent<Spawner>();
    }
    
    private void Update()
    {
#if UNITY_STANDALONE_WIN
        if(_chest.gameObject.activeInHierarchy)
            return;
        
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftControl))
        {
            _chest.gameObject.SetActive(true);
        }
#endif
#if UNITY_EDITOR
        if(_chest.gameObject.activeInHierarchy)
            return;
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            _chest.gameObject.SetActive(true);
        }
#endif
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
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SpawnDelay(1));
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
        
        var evt = LevelEvents.StageEndEvent;
        _endStageEventChannel.RaiseEvent(evt);

        var inCombatEvt = LevelEvents.InCombatCheckEvent;
        inCombatEvt.isCombat = false;
        _levelEventChannel.RaiseEvent(inCombatEvt);

        _totem.RaiseTotems();
    }
}
