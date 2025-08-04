using System.Linq;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using YH.EventSystem;

public class SpecialUpgradeCheckPanel : UpgradeCheckPanelBase
{
    [SerializeField] private GameEventChannelSO _specialNodeUpgradeEventChannel;

    protected override void Awake()
    {
        base.Awake();
        _specialNodeUpgradeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
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

    public override void Upgrade()
    {
        base.Upgrade();
        
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
        base.OpenWindow();
        
        var upgradeSkillSelectLockEvent = SpecialNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = true;
        _specialNodeUpgradeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
    }

    public override void CloseWindow()
    {
        base.CloseWindow();

        var upgradeSkillSelectLockEvent = SpecialNodeUpgradeEvents.UpgradeSkillSelectLockEvent;
        upgradeSkillSelectLockEvent.isLocked = false;
        _specialNodeUpgradeEventChannel.RaiseEvent(upgradeSkillSelectLockEvent);
    }
}
