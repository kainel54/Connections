using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YH.Players;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private Image _visual;
    
    private Player _player;
    private float _maxHealth;
    
    private Tween _tween;

    private void Awake()
    {
        _playerManagerSO.SetUpPlayerEvent += HandleSetUpPlayer;
    }

    private void OnDestroy()
    {
        _player.GetCompo<HealthCompo>().OnHealthChangedEvent.RemoveListener(HandleHealthChanged);
        _playerManagerSO.SetUpPlayerEvent -= HandleSetUpPlayer;
    }

    private void HandleSetUpPlayer()
    {
        _player = _playerManagerSO.Player;
        _maxHealth = _player.GetCompo<HealthCompo>().MaxHealth;
        
        _player.GetCompo<HealthCompo>().OnHealthChangedEvent.AddListener(HandleHealthChanged);
    }
    
    private void HandleHealthChanged(float prevHealth, float currentHealth, bool arg2)
    {
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            _tween?.Kill();
            _visual.DOFillAmount(0, 0.3f);
        }
        else
            _tween = _visual.DOFillAmount(currentHealth / _maxHealth, 0.7f);
    }
}
