using System;
using System.Collections.Generic;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;
using Random = UnityEngine.Random;

public class DefaultUpgradeCheckPanel : WindowPanel
{
    [ColorUsage(false, true)] [SerializeField] private List<Color> _colors = new List<Color>();
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private Image _skillImage;
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannel;
    
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
        
        _defaultNodeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
        _canvasGroup = GetComponent<CanvasGroup>();
        
        CloseWindow();
    }
    
    private void OnDestroy()
    {
        _defaultNodeEventChannel.RemoveListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
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
        
        int count;
        int random = Random.Range(1, 101);
        
        if (random <= 10)
            count = 4;
        else if(random <= 45)
            count = 3;
        else if(random <= 75)
            count = 2;
        else
            count = 1;
        
        var upgradeCountInitEvent = DefaultNodeUpgradeEvents.UpgradeCountInitEvent;
        upgradeCountInitEvent.count = count;
        _defaultNodeEventChannel.RaiseEvent(upgradeCountInitEvent);
        
        int upgradeCost = _selectedItem.nodeGridDictionary.Count * 10;
        _playerManagerSO.AddCoin(-upgradeCost);
        
        _submitButton.SetActive(false);
        _cancelButton.SetActive(false);
        
        UpgradeEvent?.Invoke();
    }

    public override void OpenWindow()
    {
        var upgradeSkillSelectLockEvent = DefaultNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = true;
        _defaultNodeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
        
        _submitButton.SetActive(true);
        _cancelButton.SetActive(true);
        _checkButton.SetActive(false);
        
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public override void CloseWindow()
    { 
        var upgradeSkillSelectLockEvent = DefaultNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = false;
        _defaultNodeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        
    }
}
