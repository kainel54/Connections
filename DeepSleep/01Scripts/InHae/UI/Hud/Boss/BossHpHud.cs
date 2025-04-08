using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class BossHpHud : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _levelEventChannel;
    [SerializeField] private Image _visual;
    
    private BTEnemy _boss;
    private float _maxHealth;
    
    private void Awake()
    {
        gameObject.SetActive(false);
        _levelEventChannel.AddListener<BossLevelEvent>(HandleBossHpBar);
    }

    private void OnDestroy()
    {
        _levelEventChannel.RemoveListener<BossLevelEvent>(HandleBossHpBar);

        if (_boss == null)
            return;
        
        _boss.GetCompo<HealthCompo>().OnHealthChangedEvent.RemoveListener(HandleHealthChanged);
        _boss.GetCompo<HealthCompo>().OnDieEvent.RemoveListener(HandleBossDieEvent);
    }
    
    private void HandleBossHpBar(BossLevelEvent evt)
    {
        gameObject.SetActive(true);
        
        _boss = evt.boss;
        _maxHealth = _boss.GetCompo<HealthCompo>().MaxHealth;
        
        _boss.GetCompo<HealthCompo>().OnHealthChangedEvent.AddListener(HandleHealthChanged);
        _boss.GetCompo<HealthCompo>().OnDieEvent.AddListener(HandleBossDieEvent);
    }

    private void HandleBossDieEvent()
    {
        gameObject.SetActive(false);
    }

    private void HandleHealthChanged(float prevHealth, float currentHealth, bool arg2)
    {
        if (currentHealth <= 0)
            _visual.fillAmount = 0;
        else
            _visual.DOFillAmount(currentHealth / _maxHealth, 0.7f);
    }
}
