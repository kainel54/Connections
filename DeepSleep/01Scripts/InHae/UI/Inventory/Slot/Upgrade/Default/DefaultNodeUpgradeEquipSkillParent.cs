using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;

public class DefaultNodeUpgradeEquipSkillParent : NodeUpgradeEquipSkillParent
{
    protected override void Awake()
    {
        base.Awake();
        _upgradeEventChannel.AddListener<EquipSkillSlotInitEvent>(HandleEquipSkillSlotInitEvent);
    }

    private void OnDestroy()
    {
        _upgradeEventChannel.RemoveListener<EquipSkillSlotInitEvent>(HandleEquipSkillSlotInitEvent);
    }

    private void HandleEquipSkillSlotInitEvent(EquipSkillSlotInitEvent evt)
    {
        foreach (var equipSlot in evt.slots)
        {
            _equipSlotList[equipSlot.skillIdx].Init(equipSlot);
        }
    }
}
