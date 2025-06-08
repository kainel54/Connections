using System;
using System.Collections.Generic;
using IH.EventSystem.StatusEvent;
using UnityEngine;
using YH.Entities;
using YH.EventSystem;

public class StatusManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _statusEventChannel;
    private Dictionary<StatusEnum, StatusStat> _statusStats = new ();
    
    // 캐싱용 딕셔너리 (플레이어, 보스 등 같은 대상에게 GetCompo 계속 쓰지 않게)
    private Dictionary<Entity, EntityStatus> _cashingStatus = new();

    // StatusEvent -> Manager -> Entity의 구조
    private void Awake()
    {
        foreach (var statusEnum in Enum.GetValues(typeof(StatusEnum)))
        {
            Type t = Type.GetType(statusEnum.ToString());
            StatusStat statusStat = Activator.CreateInstance(t) as StatusStat;

            _statusStats.Add((StatusEnum)statusEnum, statusStat);
        }
    }

    private void Start()
    {
        _statusEventChannel.AddListener<AddStatusEvent>(HandleAddStatusEvent);
        _statusEventChannel.AddListener<AddTimeStatusEvent>(HandleAddTimeStatusEvent);
        _statusEventChannel.AddListener<RemoveStatusEvent>(HandleRemoveStatusEvent);
        _statusEventChannel.AddListener<RemoveAllStatusEvent>(HandleRemoveAllStatusEvent);
    }

    private void OnDestroy()
    {
        _statusEventChannel.RemoveListener<AddStatusEvent>(HandleAddStatusEvent);
        _statusEventChannel.RemoveListener<AddTimeStatusEvent>(HandleAddTimeStatusEvent);
        _statusEventChannel.RemoveListener<RemoveStatusEvent>(HandleRemoveStatusEvent);
        _statusEventChannel.RemoveListener<RemoveAllStatusEvent>(HandleRemoveAllStatusEvent);
    }

    private void HandleRemoveAllStatusEvent(RemoveAllStatusEvent evt)
    {
        if(!_cashingStatus.ContainsKey(evt.entity))
            return;
        
        _cashingStatus[evt.entity].AllRemoveStatus();
    }
    
    private void HandleRemoveStatusEvent(RemoveStatusEvent evt)
    {
        if(!_cashingStatus.ContainsKey(evt.entity))
            return;
        
        _cashingStatus[evt.entity].RemoveStatus(evt.status);
    }
    
    private void HandleAddStatusEvent(AddStatusEvent evt)
    {
        if(evt.entity.IsDead)
            return;
        
        if (!_cashingStatus.ContainsKey(evt.entity))
        {
            EntityStatus entityStatus = evt.entity.GetCompo<EntityStatus>(true);
            _cashingStatus.Add(evt.entity, entityStatus);
            entityStatus.StatusDictionaryInit(_statusStats);
        }

        _cashingStatus[evt.entity].AddStatus(evt.status, true);
    }
    
    private void HandleAddTimeStatusEvent(AddTimeStatusEvent evt)
    {
        if(evt.entity.IsDead)
            return;
        
        if (!_cashingStatus.ContainsKey(evt.entity))
        {
            EntityStatus entityStatus = evt.entity.GetCompo<EntityStatus>(true);
            _cashingStatus.Add(evt.entity, entityStatus);
            entityStatus.StatusDictionaryInit(_statusStats);
        }
        
        _cashingStatus[evt.entity].AddStatus(evt.status, false, evt.time);
    }
}