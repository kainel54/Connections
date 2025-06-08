using UnityEngine;

public abstract class LevelDoorVisual : MonoBehaviour, ILevelDoorCompo
{
    protected LevelDoor _door;
    protected bool _autoOpenDoorCheck;
    
    public void Initialize(LevelDoor door)
    {
        _door = door;
    }
    
    public abstract void Open();
    public abstract void Close();
    public abstract void DoorEnable(bool isActive);
    
}
