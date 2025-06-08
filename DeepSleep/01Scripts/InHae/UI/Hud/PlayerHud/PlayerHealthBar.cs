using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.Players;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private Image _visual;
    [SerializeField] private TextMeshProUGUI _text;
    
    private Player _player;
    private Tween _tween;

    private void Awake()
    {
        _playerManagerSO.SetUpPlayerEvent += HandleSetUpPlayer;
    }

    private void OnDestroy()
    {
        _player.GetCompo<EntityHealth>().OnHealthChangedEvent.RemoveListener(HandleHealthChanged);
        _playerManagerSO.SetUpPlayerEvent -= HandleSetUpPlayer;
    }

    private void HandleSetUpPlayer()
    {
        _player = _playerManagerSO.Player;
        _player.GetCompo<EntityHealth>().OnHealthChangedEvent.AddListener(HandleHealthChanged);
        
        _text.SetText($"{_player.GetCompo<EntityHealth>().Health:0} / {_player.GetCompo<EntityHealth>().MaxHealth:0}");
    }
    
    private void HandleHealthChanged(float prevHealth, float currentHealth, bool arg2)
    {
        float maxHealth = _player.GetCompo<EntityHealth>().MaxHealth;
        if (currentHealth <= 0)
        {
            _tween?.Kill();
            _visual.DOFillAmount(0, 0.3f);
        }
        else
        {
            _tween = _visual.DOFillAmount(currentHealth / maxHealth, 0.7f);
        }
        _text.SetText($"{currentHealth:0} / {maxHealth:0}");
    }
}
