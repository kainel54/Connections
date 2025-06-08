using System;
using DG.Tweening;
using IH.EventSystem.InteractEvent;
using IH.Manager;
using UnityEngine;
using YH.EventSystem;

public class StartSelectItem : Interactable, ISpecialInitItem
{
    [SerializeField] private GameEventChannelSO _interactEventChannelSO;
    [SerializeField] private float _upTime;
    [SerializeField] private Transform _visualTrm;
    [HideInInspector] public SkillItemSO skillItem;

    public bool isCollected;
    private bool _anotherItemSelected;
    
    private Collider _collider;
    private Transform _currentVisual;

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
        transform.DOMoveY(y, _upTime).SetEase(Ease.OutQuint);
    }
}
