using System;
using System.Linq;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;

public class SpecialUpgradeCheckPanel : WindowPanel
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private Image _skillImage;
    [SerializeField] private GameEventChannelSO _specialNodeUpgradeEventChannel;
    
    private GameObject _submitButton;
    private GameObject _cancelButton;
    private GameObject _checkButton;
    
    private SkillInventoryItem _selectedItem;
    private CanvasGroup _canvasGroup;
    
    private readonly int _colorHash = Shader.PropertyToID("_Color");

    public event Action UpgradeEvent;

    private void Awake()
    {
        _submitButton = transform.Find("Buttons/SubmitButton").gameObject;
        _cancelButton = transform.Find("Buttons/CancelButton").gameObject;
        _checkButton = transform.Find("Buttons/CheckButton").gameObject;
        
        _specialNodeUpgradeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
        _canvasGroup = GetComponent<CanvasGroup>();
        
        CloseWindow();
    }
    
    private void OnDestroy()
    {
        _specialNodeUpgradeEventChannel.RemoveListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
    }
    
    private void HandleNodeUpgradeSkillInfo(UpgradeSkillSelectEvent evt)
    {
        _selectedItem = evt.item;
        SkillItemSO skillItemSO = _selectedItem.data as SkillItemSO;
        _skillImage.sprite = skillItemSO.icon;
    }

    public override void HandleCloseUI()
    {
        var uiLockEvent = UIPanelEvent.WindowPanelLockEvent;
        uiLockEvent.isOpenLocked = false;
        _uiEventChannel.RaiseEvent(uiLockEvent);
        
        var evt = UIPanelEvent.WindowPanelToggleEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);
    }

    public void Upgrade()
    {
        var uiLockEvent = UIPanelEvent.WindowPanelLockEvent;
        uiLockEvent.isOpenLocked = true;
        _uiEventChannel.RaiseEvent(uiLockEvent);
        
        int specialNodeCount = _selectedItem.nodeGridDictionary.Values.Count(x => x.isSpecial);
        int upgradeCost = specialNodeCount * 50;
        if (specialNodeCount == 0)
            upgradeCost = 25;
        
        var evt = SpecialNodeUpgradeEvents.UpgradeNodeInitEvent;
        _specialNodeUpgradeEventChannel.RaiseEvent(evt);
        
        _playerManagerSO.AddCoin(-upgradeCost);
        
        _submitButton.SetActive(false);
        _cancelButton.SetActive(false);
        
        UpgradeEvent?.Invoke();
    }

    public override void OpenWindow()
    {
        var upgradeSkillSelectLockEvent = SpecialNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = true;
        _specialNodeUpgradeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
        
        _submitButton.SetActive(true);
        _cancelButton.SetActive(true);
        _checkButton.SetActive(false);
        
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public override void CloseWindow()
    {
        var upgradeSkillSelectLockEvent = SpecialNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = false;
        _specialNodeUpgradeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
}
