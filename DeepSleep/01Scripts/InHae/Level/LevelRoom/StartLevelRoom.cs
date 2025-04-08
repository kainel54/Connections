using System;
using UnityEngine;

public class StartLevelRoom : LevelRoom
{
    private void Start()
    {
        isClear = true;
        ConnectDoorDisable();
    }
}
