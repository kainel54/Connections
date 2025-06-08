using IH.EventSystem.StatusEvent;
using UnityEngine;
using YH.EventSystem;

public class PlayerStatus : EntityStatus
{
    [SerializeField] private GameEventChannelSO _statusEventChannel;
    
    public override void AddStatus(StatusEnum statusEnum, bool isInfinity, float time = 0)
    {
        base.AddStatus(statusEnum, isInfinity, time);

        if (isInfinity)
        {
            var sendEvt = StatusEvents.PlayerIsAddedStatusEvent;
            sendEvt.status = _statusDictionary[statusEnum];
            sendEvt.type = statusEnum;
            _statusEventChannel.RaiseEvent(sendEvt);
        }
        else
        {
            var sendEvt = StatusEvents.PlayerIsAddedTimeStatusEvent;
            sendEvt.status = _statusDictionary[statusEnum];
            sendEvt.type = statusEnum;
            _statusEventChannel.RaiseEvent(sendEvt);
        }
    }

    public override void RemoveStatus(StatusEnum statusEnum)
    {
        base.RemoveStatus(statusEnum);
        
        var sendEvt = StatusEvents.PlayerIsRemovedStatusEvent;
        sendEvt.status = _statusDictionary[statusEnum];
        sendEvt.type = statusEnum;
        _statusEventChannel.RaiseEvent(sendEvt);
    }
}
