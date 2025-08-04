using System;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;

public abstract class UpgradeSkillInfouIBase : MonoBehaviour
{
    [SerializeField] protected GameEventChannelSO _skillNodeEventChannelSO;

    [SerializeField] protected PlayerManagerSO _playerManagerSO;
    [SerializeField] protected WindowPanel _upgradeWindow;
    [SerializeField] protected Image _skillImage;
    
    protected TextMeshProUGUI _title;
    protected TextMeshProUGUI _priceText;
    protected TextMeshProUGUI _attackTypeAndTypeText;
    
    protected Sprite _defaultSprite;

    protected bool _isUpgradeAble;
    protected SkillInventoryItem _selectedSkillItem;
    
    protected SkillResultDescription _skillResultDescription;

    protected virtual void Awake()
    {
        _skillResultDescription = GetComponentInChildren<SkillResultDescription>();

        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _priceText = transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
        _attackTypeAndTypeText = transform.Find("AttackTypeAndTypeText").GetComponent<TextMeshProUGUI>();
        
        _defaultSprite = _skillImage.sprite;
        
        _skillNodeEventChannelSO.AddListener<SkillStatViewInitEvent>(SetUpInfo);
    }

    protected virtual void OnDestroy()
    {
        _skillNodeEventChannelSO.RemoveListener<SkillStatViewInitEvent>(SetUpInfo);
    }

    protected virtual void InfoInit()
    {
        _skillResultDescription.Init();
        _title.SetText("");
        _priceText.SetText("");
        _attackTypeAndTypeText.SetText("");
        
        _selectedSkillItem = null;
        
        _isUpgradeAble = false;
        _skillImage.sprite = _defaultSprite;
    }
    
    protected virtual void SetUpInfo(SkillStatViewInitEvent evt)
    {
        if(_selectedSkillItem == null)
            return;
        
        SkillItemSO skillItemSO = _selectedSkillItem.data as SkillItemSO;
        _title.SetText(skillItemSO.itemName);
        _skillImage.sprite = skillItemSO.icon;
    }

    protected void SetDescription()
    {
        var setSkillResultDescriptionEvt = SkillNodeEvents.SetSkillResultDescriptionEvent;
        setSkillResultDescriptionEvt.skillInventoryItem = _selectedSkillItem;
        setSkillResultDescriptionEvt.targetDescription = _skillResultDescription;
        setSkillResultDescriptionEvt.attackTypeAndTypeText = _attackTypeAndTypeText;
        _skillNodeEventChannelSO.RaiseEvent(setSkillResultDescriptionEvt);
    }
    
    public void OpenUpgradeWindow()
    {
        if (!_isUpgradeAble)
            return;
        
        _upgradeWindow.HandleOpenUI();
    }
}
