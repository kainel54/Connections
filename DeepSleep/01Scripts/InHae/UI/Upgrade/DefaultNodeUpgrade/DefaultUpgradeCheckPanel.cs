using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using UnityEngine;
using YH.EventSystem;
using Random = UnityEngine.Random;

public class DefaultUpgradeCheckPanel : UpgradeCheckPanelBase
{
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannel;

    protected override void Awake()
    {
        base.Awake();
        _defaultNodeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
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

    public override void Upgrade()
    {
        base.Upgrade();
        
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
        base.OpenWindow();
        
        var upgradeSkillSelectLockEvent = DefaultNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = true;
        _defaultNodeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
    }

    public override void CloseWindow()
    { 
        base.CloseWindow();
        
        var upgradeSkillSelectLockEvent = DefaultNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = false;
        _defaultNodeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
    }
}
