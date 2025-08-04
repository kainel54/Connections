using DG.Tweening;
using IH.EventSystem.InteractEvent;
using IH.Manager;
using ObjectPooling;
using UnityEngine;
using YH.EventSystem;

public class StartSelectItem : Interactable, ISpecialInitItem
{
    [SerializeField] private GameEventChannelSO _interactEventChannelSO;
    [SerializeField] private float _upTime;
    [SerializeField] private Transform _visualTrm;
    [HideInInspector] public SkillItemSO skillItem;

    private ItemDataSO _itemData;
    
    public bool isCollected;
    private bool _anotherItemSelected;
    
    private Collider _collider;
    private Transform _currentVisual;

    private PoolingNoLifeTimeEffectPlayer _glowEffect;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public override void Interact()
    {
        if(_anotherItemSelected)
            return;

        isCollected = true;
        base.Interact();
        _collider.enabled = false;
        if (InventoryManager.Instance.CanAddItem(skillItem))
        {
            InventoryManager.Instance.AddInventoryItemWithSo(skillItem);
            
            if(_glowEffect != null)
                PoolManager.Instance.Push(_glowEffect, true);
            
            Destroy(_currentVisual.gameObject);
        }
    }

    public override void EnableDescription()
    {
        var skillInteractDescriptionPanel = InteractEvents.SkillInteractDescriptionPanelEvent;
        skillInteractDescriptionPanel.isPanelActive = true;
        skillInteractDescriptionPanel.position = _visualTrm.position;
        skillInteractDescriptionPanel.itemDataSo = skillItem;
        
        _interactEventChannelSO.RaiseEvent(skillInteractDescriptionPanel);
    }

    public override void DisableDescription()
    {
        var skillInteractDescriptionPanel = InteractEvents.SkillInteractDescriptionPanelEvent;
        skillInteractDescriptionPanel.isPanelActive = false;
        skillInteractDescriptionPanel.position = _visualTrm.position;
        skillInteractDescriptionPanel.itemDataSo = skillItem;
        
        _interactEventChannelSO.RaiseEvent(skillInteractDescriptionPanel);
    }

    public void SpecialInit(ItemDataSO dataSo)
    {
        skillItem = dataSo as SkillItemSO;
        PlayGlowEffect();
    }
    
    public void VisualInit()
    {
        _currentVisual = Instantiate(skillItem.visual, transform).transform;
        Vector3 pos = _visualTrm.position;
        _currentVisual.position = pos;
    }

    public void NoSelectable()
    {
        _collider.enabled = false;
        _anotherItemSelected = true;
        float y = transform.position.y;
        y += 20f;
        transform.DOMoveY(y, _upTime).SetEase(Ease.OutQuint)
            .OnComplete(() =>
            {
                if(_glowEffect != null)
                    PoolManager.Instance.Push(_glowEffect, true);
            });
    }
    
    private void PlayGlowEffect()
    {
        if(skillItem.itemTier == ItemTier.Normal)
            return;

        switch (skillItem.itemTier)
        {
            case ItemTier.Rare:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemRareGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
            case ItemTier.Epic:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemEpicGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
            case ItemTier.Legendary:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemLegendaryGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
        }

        _glowEffect.PlayEffect(_visualTrm.position, Quaternion.identity, Vector3.one, transform);
    }
}
