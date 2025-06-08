using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.LevelEvent;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.Manager;
using UnityEngine;
using YH.EventSystem;
using Special = IH.EventSystem.NodeEvent.SpecialPartNodeEvent;

public class SkillEquipSlotParent : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannelSO;
    [SerializeField] private GameEventChannelSO _defaultNodeUpgradeEventChannelSO;
    [SerializeField] private GameEventChannelSO _specialNodeUpgradeEventChannelSO;
    [SerializeField] private GameEventChannelSO _levelEventChannelSO;
    
    private List<SkillEquipSlot> _skillSlots = new List<SkillEquipSlot>();
    private bool _isCombat;

    private void Awake()
    {
        _skillSlots = GetComponentsInChildren<SkillEquipSlot>().ToList();
        
        _skillNodeEventChannelSO.AddListener<SkillAutoEquipEvent>(HandleSkillAutoEquip);
        _defaultNodeUpgradeEventChannelSO.AddListener<UpgradeSkillInitEvent>(HandleDefaultNodeUpgradeInitEvent);
        _specialNodeUpgradeEventChannelSO.AddListener<Special.UpgradeSkillInitEvent>
            (HandleSpecialNodeUpgradeInitEvent);
        
        _levelEventChannelSO.AddListener<InCombatCheckEvent>(HandleInCombatCheck);
    }

    private void OnDestroy()
    {
        _skillNodeEventChannelSO.RemoveListener<SkillAutoEquipEvent>(HandleSkillAutoEquip);
        _defaultNodeUpgradeEventChannelSO.RemoveListener<UpgradeSkillInitEvent>(HandleDefaultNodeUpgradeInitEvent);
        _specialNodeUpgradeEventChannelSO.RemoveListener<Special.UpgradeSkillInitEvent>
            (HandleSpecialNodeUpgradeInitEvent);
        
        _levelEventChannelSO.RemoveListener<InCombatCheckEvent>(HandleInCombatCheck);
    }
    
    private void HandleInCombatCheck(InCombatCheckEvent evt)
    {
        _isCombat = evt.isCombat;
    }

    private void HandleSpecialNodeUpgradeInitEvent(Special.UpgradeSkillInitEvent evt)
    {
        var equipSkillSlotInitEvt = Special.SpecialNodeUpgradeEvents.EquipSkillSlotInitEvent;
        equipSkillSlotInitEvt.slots = _skillSlots;
        _specialNodeUpgradeEventChannelSO.RaiseEvent(equipSkillSlotInitEvt);
    }
    
    private void HandleDefaultNodeUpgradeInitEvent(UpgradeSkillInitEvent evt)
    {
        var equipSkillSlotInitEvt = DefaultNodeUpgradeEvents.EquipSkillSlotInitEvent;
        equipSkillSlotInitEvt.slots = _skillSlots;
        _defaultNodeUpgradeEventChannelSO.RaiseEvent(equipSkillSlotInitEvt);
    }

    private void HandleSkillAutoEquip(SkillAutoEquipEvent evt)
    {
        if(_isCombat)
            return;
        
        List<SkillEquipSlot> equipSlotList = _skillSlots
            .Where(x => x.isEmpty).OrderBy(x => x.skillIdx).ToList();
        
        if(equipSlotList.Count == 0)
            return;
        
        equipSlotList[0].UpdateSlot(evt.skillInventoryItem);
        InventoryManager.Instance.RemoveInventoryItem(InventoryType.Skill, evt.skillInventoryItem);
    }
}
