using IH.EventSystem.LevelEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public class LevelDoorMove : MonoBehaviour, ILevelDoorCompo
{
    [SerializeField] private GameEventChannelSO _levelChannel;
    private LevelDoor _levelDoor;
    
    public void Initialize(LevelDoor door)
    {
        _levelDoor = door;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!_levelDoor.CanDoorOpen)
            return;
        
        if (other.TryGetComponent(out Player player))
        {
            Move();
        }
    }
    
    private void Move()
    {
        var evt = LevelEvents.BasicLevelMoveEvent;
        evt.enterDoorDir = _levelDoor.GetDir();
        _levelChannel.RaiseEvent(evt);
    }
}
