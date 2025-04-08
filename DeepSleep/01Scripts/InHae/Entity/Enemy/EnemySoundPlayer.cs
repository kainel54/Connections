using System;
using UnityEngine;
using YH.Entities;
using YH.EventSystem;

public class EnemySoundPlayer : MonoBehaviour, IEntityComponent
{
    [SerializeField] private GameEventChannelSO _soundChannel;
    
    [Header("Health Sound")] 
    [SerializeField] private SoundSO _hitSound;
    [SerializeField] private SoundSO _dieSound;
    
    private BTEnemy _enemy;
    
    private float _currentHitDelay;
    private float _hitDelay = 0.01f;
    
    public void Initialize(Entity entity)
    {
        _enemy = entity as BTEnemy;
    }

    private void Awake()
    {
        _enemy.GetCompo<HealthCompo>().OnHealthChangedEvent.AddListener(HandleHitSound);
        _enemy.GetCompo<HealthCompo>().OnDieEvent.AddListener(HandleDeadSound);
    }

    private void OnDestroy()
    {
        _enemy.GetCompo<HealthCompo>().OnHealthChangedEvent.RemoveListener(HandleHitSound);
        _enemy.GetCompo<HealthCompo>().OnDieEvent.RemoveListener(HandleDeadSound);
    }
    
    private void Update()
    {
        if (_currentHitDelay > 0)
        {
            _currentHitDelay -= Time.deltaTime;
        }
    }
    
    private void HandleDeadSound()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _dieSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
    }
    
    private void HandleHitSound(float arg0, float arg1, bool arg2)
    {
        if(_currentHitDelay > 0)
            return;
        
        _currentHitDelay = _hitDelay;
        
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _hitSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
    }
}
