using IH.EventSystem.MissionEvent;
using UnityEngine;
using YH.EventSystem;

public abstract class SpecialLevelMission : MonoBehaviour
{
    [SerializeField] protected GameEventChannelSO _missionEventChannel;
    [SerializeField] private string _description;
    
    protected SpecialLevelRoom _specialLevelRoom;
    protected bool _missionEnd;
    
    public void SetRoom(SpecialLevelRoom room)
    {
        _specialLevelRoom = room;
    }

    public virtual void Init()
    {
        var evt = MissionEvents.MissionInitEvent;
        evt.missionDescription = _description;
        _missionEventChannel.RaiseEvent(evt);
    }
}
