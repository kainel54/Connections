using UnityEngine;

public class StartLevelRoom : LevelRoom
{
    public Transform playerSpawnPoint;
    
    private void Start()
    {
        LevelClear();
    }
}
