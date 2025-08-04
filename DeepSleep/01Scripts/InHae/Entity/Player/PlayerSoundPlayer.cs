using System.Collections.Generic;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.Entities;
using YH.EventSystem;
using YH.Players;
using Random = UnityEngine.Random;

public class PlayerSoundPlayer : MonoBehaviour, IEntityComponent
{
    [SerializeField] private GameEventChannelSO _soundChannel;

    [Header("Health Sound")] 
    [SerializeField] private List<SoundSO> _hitSound;
    [SerializeField] private SoundSO _dieSound;


    [Header("Attack Sound")]
    [SerializeField] private List<SoundSO> _attackSound;

    [SerializeField] private SoundSO _dashSound;
    private Player _player;
    private MOBAPlayer _mobaPlayer;

    private float _currentHitDelay;
    private float _hitDelay = 0.3f;
    
    public void Initialize(Entity entity)
    {
        _player = entity as Player;
        if (_player.PlayerInput.ControlType == ControlType.PointNClick)
        {
            _mobaPlayer = _player as MOBAPlayer;
            _mobaPlayer.AttackComboEvent += HandleAttackEvent;
            _mobaPlayer.DashEvent += HandleDashEvent;
        }
    }
    private void HandleAttackEvent(int attackCombo)
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _attackSound[attackCombo];
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt);
    }

    private void HandleDashEvent()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _dashSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt);
    }

    private void OnDestroy()
    {
    }

    private void Update()
    {
        if (_currentHitDelay > 0)
        {
            _currentHitDelay -= Time.deltaTime;
        }
    }

    
    public void HandleDeadSound()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _dieSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
    }
    
    public void HandleHitSound()
    {
        if(_currentHitDelay > 0)
            return;
        
        _currentHitDelay = _hitDelay;
        
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _hitSound[Random.Range(0, _hitSound.Count)];
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
    }
}

