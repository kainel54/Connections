using System;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.SoundEvent;
using ObjectPooling;
using UnityEngine;
using YH.EventSystem;

public class PartNodeEquipEffect : MonoBehaviour, IPoolable
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannelSO;

    [SerializeField] private Color _specialColor;
    [SerializeField] private EffectPoolingType _type;
    public GameObject GameObject => gameObject;
    public Enum PoolEnum => _type;
    
    private ParticleSystem _particleSystem;

    private bool _isNodePanelOpen;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        
        _skillNodeEventChannelSO.AddListener<EquipSkillSelectEvent>(HandleEquipSkillSelect);
    }

    private void OnDestroy()
    {
        _skillNodeEventChannelSO.RemoveListener<EquipSkillSelectEvent>(HandleEquipSkillSelect);
    }

    private void HandleEquipSkillSelect(EquipSkillSelectEvent evt)
    {
        _isNodePanelOpen = evt.isSelected;
    }

    public void OnParticleSystemStopped()
    {
        PoolManager.Instance.Push(this, true);
    }

    public void Init(Transform parent, bool isSpecial)
    {
        if (!_isNodePanelOpen)
        {
            OnParticleSystemStopped();
            return;
        }
        
        var main = _particleSystem.main;
        main.startColor = isSpecial ? _specialColor : Color.white;
        
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0, -10f);
        transform.localScale = Vector3.one * 0.4f;
        
        _particleSystem.Play();
    }

    public void OnPop()
    {
    }

    public void OnPush()
    {
        _particleSystem.Clear();
    }
}
