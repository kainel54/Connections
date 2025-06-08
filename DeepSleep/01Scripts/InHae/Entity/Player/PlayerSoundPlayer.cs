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

    [Header("Gun Sound")] 
    [SerializeField] private SoundSO _shootSound;
    [SerializeField] private SoundSO _dashSound;
    
    [Header("Health Sound")] 
    [SerializeField] private List<SoundSO> _hitSound;
    [SerializeField] private SoundSO _dieSound;

    private Player _player;

    private float _currentHitDelay;
    private float _hitDelay = 0.3f;
    
    public void Initialize(Entity entity)
    {
        _player = entity as Player;
    }

    private void Awake()
    {
        _player.GetCompo<PlayerAttackCompo>().FireEvent += HandleFireSound;
        _player.GetCompo<PlayerMovement>().OnDashEvent += HandleDashSound;
    }

    private void HandleDashSound(bool obj)
    {
        if (obj)
        {
            var evt = SoundEvents.PlaySfxEvent;
            evt.clipData = _dashSound;
            evt.position = transform.position;

            _soundChannel.RaiseEvent(evt);
        }
    }

    private void OnDestroy()
    {
        _player.GetCompo<PlayerAttackCompo>().FireEvent -= HandleFireSound;
    }

    private void Update()
    {
        if (_currentHitDelay > 0)
        {
            _currentHitDelay -= Time.deltaTime;
        }
    }

    private void HandleReloadSound(float obj)
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _dashSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt);
    }

    private void HandleFireSound()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _shootSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt);
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

