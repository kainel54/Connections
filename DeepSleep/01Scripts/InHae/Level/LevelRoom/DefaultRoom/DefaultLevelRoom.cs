using UnityEngine;

public class DefaultLevelRoom : LevelRoom
{
    [SerializeField] private DefaultRoomChest _chest;
    private Spawner _spawner;

    private void Start()
    {
        _spawner = GetComponent<Spawner>();
        _spawner.levelClearEvent += HandleLevelClearEvent;
    }

    private void OnDestroy()
    {
        if(_spawner != null)
            _spawner.levelClearEvent -= HandleLevelClearEvent;
    }


    public override void EnterEvent()
    {
        if(isClear)
            return;

        StartSpawn();
    }

    private void StartSpawn()
    {
        _spawner.Spawn();
    }

    private void HandleLevelClearEvent()
    {
        if (isClear)
            return;

        LevelClear();
    }

    public override void LevelClear()
    {
        base.LevelClear();
        _chest.Open();
    }
}
