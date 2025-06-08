using System.Collections.Generic;
using IH.EventSystem.StatusEvent;
using UnityEngine;
using YH.EventSystem;

public class PlayerStatusHudUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _statusEventChannel;
    [SerializeField] private List<StatusDataSO> _statusData;
    [SerializeField] private PlayerStatusIcon _playerStatusIcon;
    
    private Dictionary<StatusEnum, StatusDataSO> _statusDictionary;
    private Dictionary<StatusEnum, PlayerStatusIcon> _statusIconDictionary = new Dictionary<StatusEnum, PlayerStatusIcon>();

    private void Awake()
    {
        _statusDictionary = new Dictionary<StatusEnum, StatusDataSO>();
        _statusData.ForEach(x => _statusDictionary.Add(x.status, x));
    }

    private void Start()
    {
        _statusEventChannel.AddListener<PlayerIsAddedStatusEvent>(HandleAddStatusEvent);
        _statusEventChannel.AddListener<PlayerIsRemovedStatusEvent>(HandleRemoveStatusEvent);
        _statusEventChannel.AddListener<PlayerIsAddedTimeStatusEvent>(HandleAddTimeStatusEvent);
    }
    
    private void OnDestroy()
    {
        _statusEventChannel.RemoveListener<PlayerIsAddedStatusEvent>(HandleAddStatusEvent);
        _statusEventChannel.RemoveListener<PlayerIsRemovedStatusEvent>(HandleRemoveStatusEvent);
        _statusEventChannel.RemoveListener<PlayerIsAddedTimeStatusEvent>(HandleAddTimeStatusEvent);
    }
    
    
    private void HandleAddTimeStatusEvent(PlayerIsAddedTimeStatusEvent evt)
    {
        if (!_statusIconDictionary.ContainsKey(evt.type))
        {
            PlayerStatusIcon icon = Instantiate(_playerStatusIcon, transform);
            _statusIconDictionary.Add(evt.type, icon);
            icon.onStatusEnd += HandleRemoveDictionary;
        }

        _statusIconDictionary[evt.type].Init(evt.status, _statusDictionary[evt.type], false);
    }

    private void HandleRemoveStatusEvent(PlayerIsRemovedStatusEvent evt)
    {
        if (!_statusIconDictionary.ContainsKey(evt.type))
            return;
        
        _statusIconDictionary[evt.type].RemoveStatus();
    }

    private void HandleAddStatusEvent(PlayerIsAddedStatusEvent evt)
    {
        if (!_statusIconDictionary.ContainsKey(evt.type))
        {
            PlayerStatusIcon icon = Instantiate(_playerStatusIcon, transform);
            _statusIconDictionary.Add(evt.type, icon);
            icon.onStatusEnd += HandleRemoveDictionary;
        }

        _statusIconDictionary[evt.type].Init(evt.status, _statusDictionary[evt.type], true);
    }
    
    private void HandleRemoveDictionary(StatusEnum type)
    {
        _statusIconDictionary[type].onStatusEnd -= HandleRemoveDictionary;
        _statusIconDictionary.Remove(type);
    }
}