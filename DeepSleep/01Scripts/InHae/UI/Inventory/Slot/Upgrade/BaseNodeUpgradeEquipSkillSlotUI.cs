using System;
using IH.EventSystem.UIEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;

public abstract class BaseNodeUpgradeEquipSkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Default Upgrade or Special Upgrade")]
    [SerializeField] protected GameEventChannelSO _upgradeEventChannel;
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    [SerializeField] private Image _skillImage;
    [SerializeField] private TextMeshProUGUI _skillKeyText;
    public int index;

    protected SkillInventoryItem _currentSkillItem;
    
    [SerializeField] private PlayerManagerSO _playerManagerSo;

    protected bool _isEmpty => _currentSkillItem == null || _currentSkillItem.data == null;
    protected bool _isLocked;

    protected virtual void Awake()
    {
        _playerManagerSo.SetUpPlayerEvent += HandleSetUpPlayer;
    }
    
    protected virtual void OnDestroy()
    {
        _playerManagerSo.SetUpPlayerEvent -= HandleSetUpPlayer;
    }
    
    private void HandleSetUpPlayer()
    {
        _skillKeyText.SetText(_playerManagerSo.Player.PlayerInput.GetSkillKeyName(index));
    }

    public void Init(SkillEquipSlot slot)
    {
        if (slot == null || slot.IsEmpty)
            return;
        
        _currentSkillItem = slot.currentSkillItem;
        _skillImage.sprite = _currentSkillItem.data.icon;
    }
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(_isEmpty)
            return;
        
        var itemSlotActiveEvent = UIEvents.ItemSlotSelectActiveEvent;
        itemSlotActiveEvent.isActive = true;
        _uiEventChannel.RaiseEvent(itemSlotActiveEvent);
        
        var itemSlotSelectEvent = UIEvents.ItemSlotSelectEvent;
        itemSlotSelectEvent.targetTrm = transform as RectTransform;
        _uiEventChannel.RaiseEvent(itemSlotSelectEvent);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(_isEmpty)
            return;
        
        var itemSlotActiveEvent = UIEvents.ItemSlotSelectActiveEvent;
        itemSlotActiveEvent.isActive = false;
        _uiEventChannel.RaiseEvent(itemSlotActiveEvent);
    }
}