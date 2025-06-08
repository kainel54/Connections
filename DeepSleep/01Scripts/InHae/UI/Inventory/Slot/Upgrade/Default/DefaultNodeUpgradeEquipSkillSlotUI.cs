using System;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultNodeUpgradeEquipSkillSlotUI : BaseNodeUpgradeEquipSkillSlotUI, IPointerClickHandler
{
    private void Awake()
    {
        _upgradeEventChannel.AddListener<UpgradeSkillSelectLockEvent>(HandleLockChange);
    }

    private void OnDestroy()
    {
        _upgradeEventChannel.RemoveListener<UpgradeSkillSelectLockEvent>(HandleLockChange);
    }
    
    private void HandleLockChange(UpgradeSkillSelectLockEvent evt)
    {
        _isLocked = evt.isLocked;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(_isEmpty || _isLocked)
            return;

        var selectImageEvt = DefaultNodeUpgradeEvents.UpgradeSlotSelectImageEvent;
        selectImageEvt.targetTrm = transform as RectTransform;
        _upgradeEventChannel.RaiseEvent(selectImageEvt);
        
        var evt = DefaultNodeUpgradeEvents.UpgradeSkillSelectEvent;
        evt.item = _currentSkillItem;
        _upgradeEventChannel.RaiseEvent(evt);
    }
}
