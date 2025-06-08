using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpecialNodeUpgradeEquipSkillSlotUI : BaseNodeUpgradeEquipSkillSlotUI, IPointerClickHandler
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

        var selectImageEvt = SpecialNodeUpgradeEvents.UpgradeSlotSelectImageEvent;
        selectImageEvt.targetTrm = transform as RectTransform;
        _upgradeEventChannel.RaiseEvent(selectImageEvt);
        
        var evt = SpecialNodeUpgradeEvents.UpgradeSkillSelectEvent;
        evt.item = _currentSkillItem;
        _upgradeEventChannel.RaiseEvent(evt);
    }
}
