using System;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.Manager;
using ObjectPooling;
using TMPro;
using UnityEngine;
using YH.EventSystem;

public class SpecialPartNodeUI : PartNodeUI
{
    [SerializeField] private GameEventChannelSO _specialPartNodeEventChannel;
    [SerializeField] private TextMeshProUGUI _transitionGuideText;
    public Color specialSlotColor;
    
    private NodeAbilityInventoryItem _currentAbility => CurrentEquipData.nodeAbilityInventoryItem;
    public bool isAbilityEmpty => _currentAbility == null || _currentAbility.data == null;
    
    private SpecialPartNodeUIOnPopUp _specialPartNodeUIOnPopUp;
    private SpecialPartNodeUIEquipProcess _specialPartNodeUIEquipProcess;

    protected override void Awake()
    {
        base.Awake();
        _specialPartNodeUIOnPopUp = GetCompo<SpecialPartNodeUIOnPopUp>();
        _specialPartNodeUIEquipProcess = GetCompo<SpecialPartNodeUIEquipProcess>();
    }

    private void Start()
    {
        _specialPartNodeEventChannel.AddListener<SetSpecialModeEvent>(HandleSpecialModeChange);
        _specialPartNodeEventChannel.AddListener<ChangeSpecialModeEvent>(HandleChangeSpecialMode);
    }
    
    private void OnDestroy()
    {
        _specialPartNodeEventChannel.RemoveListener<SetSpecialModeEvent>(HandleSpecialModeChange);
        _specialPartNodeEventChannel.RemoveListener<ChangeSpecialModeEvent>(HandleChangeSpecialMode);
    }
    
    private void HandleSpecialModeChange(SetSpecialModeEvent evt)
    {
        _specialPartNodeUIOnPopUp.PopUpChange(isSpecialMode);
    }
    
    private void HandleChangeSpecialMode(ChangeSpecialModeEvent evt)
    {
        _specialPartNodeUIOnPopUp.PopUpChange(isSpecialMode);
    }

    public override void UpdateNode(NodeEquipData nodeEquipData)
    {
        if (CurrentEquipData == null)
            CurrentEquipData = nodeEquipData;
        
        if (!isSpecialMode)
        {
            base.UpdateNode(nodeEquipData);
        }
        else
        {
            var effect = PoolManager.Instance.Pop(EffectPoolingType.PartNodeEquipEffect) as PartNodeEquipEffect;
            effect.Init(transform, true);
        }
        
        SpecialModeChangedAction?.Invoke(isSpecialMode);
    }

    public void NodeAbilityChange(NodeAbilityInventoryItem changeItem)
    {
        if (CurrentEquipData == null)
        {
            NodeEquipData nodeEquipData = new NodeEquipData
            {
                nodeAbilityInventoryItem = changeItem
            };
            skillNode.currentSkillItem.equipNodeData[index] = nodeEquipData;
            CurrentEquipData = nodeEquipData;
        }
        
        skillNode.currentSkillItem.equipNodeData[index].nodeAbilityInventoryItem = changeItem;
        CurrentEquipData.nodeAbilityInventoryItem = changeItem;
        
        SpecialModeChangedAction?.Invoke(isSpecialMode);
        
        if(isAbilityEmpty)
            return;
        var effect = PoolManager.Instance.Pop(EffectPoolingType.PartNodeEquipEffect) as PartNodeEquipEffect;
        effect.Init(transform, true);
    }

    public void ReturnNodeAbility()
    {
        if(isAbilityEmpty || !isSpecialMode)
            return;
        
        _specialPartNodeUIEquipProcess.HandleUnEquipNode();
        
        InventoryManager.Instance.AddInventoryItemWithSo(_currentAbility.data);
        skillNode.currentSkillItem.equipNodeData[index].nodeAbilityInventoryItem = null;
        SpecialModeChangedAction?.Invoke(isSpecialMode);
        
        var popUpPanel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
        popUpPanel.EndPopUp();
        
        _transitionGuideText.gameObject.SetActive(false);
    }

    public override void DisablePart()
    {
        base.DisablePart();
        if (isSpecialMode && isAbilityEmpty)
            UpdateSlotImage(null, specialSlotColor);
    }

    protected override void EnablePart()
    {
        base.EnablePart();
        if (isSpecialMode && isAbilityEmpty)
            UpdateSlotImage(null, specialSlotColor);
    }

    public void ShowTransitionGuide(bool isActive)
    {
        _transitionGuideText.gameObject.SetActive(isActive);
    }
}
