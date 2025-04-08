using UnityEngine;

public class BlacksmithLevelRoom : LevelRoom
{
    private void Start()
    {
        isClear = true;
        ConnectDoorDisable();
    }
}
