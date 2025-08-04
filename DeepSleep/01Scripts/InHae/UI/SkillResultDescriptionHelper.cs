using System;
using System.Collections.Generic;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using TMPro;
using UnityEngine;
using YH.EventSystem;

public class SkillResultDescriptionHelper : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannelSO;

    private Dictionary<string, Type> _skillTypes = new ();
    private Skill _currentPreviewSkill;

    private SkillResultDescription _currentDescription;
    private TextMeshProUGUI _currentAttackTypeAndTypeText;
    
    
    private void Awake()
    {
        _skillNodeEventChannelSO.AddListener<SetSkillResultDescriptionEvent>(HandleSetSkillResultDescription);
        _skillNodeEventChannelSO.AddListener<SkillStatViewInitEvent>(HandleSkillStatViewInit);

    }
    
    private void OnDestroy()
    {
        _skillNodeEventChannelSO.RemoveListener<SetSkillResultDescriptionEvent>(HandleSetSkillResultDescription);
        _skillNodeEventChannelSO.RemoveListener<SkillStatViewInitEvent>(HandleSkillStatViewInit);
    }

    private void HandleSetSkillResultDescription(SetSkillResultDescriptionEvent evt)
    {
        PreviewSkillInit(evt.skillInventoryItem);
        
        _currentDescription = evt.targetDescription;
        _currentAttackTypeAndTypeText = evt.attackTypeAndTypeText;
    }
    
    private void HandleSkillStatViewInit(SkillStatViewInitEvent evt)
    {
        if (_currentDescription == null)
            return;
        
        SkillItemSO skillItemSo = evt.skillInventoryItem.data as SkillItemSO;
        _currentDescription.ResultDescription(skillItemSo, evt.skill);
        _currentDescription = null;
        
        if(_currentAttackTypeAndTypeText == null)
            return;
        
        GenericSkillDataSO genericSkillData = evt.skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;
        SkillType type = genericSkillData.skillType;
        
        string attackType = genericSkillData.attackType == SkillAttackType.Melee ? "근접" : "원거리";
        string skillType = EnumStringManager.Instance.GetString(type);
        string skillTypeColor = EnumColorManager.Instance.GetStringColor(type);

        _currentAttackTypeAndTypeText.text = attackType + " " + $"<color=#{skillTypeColor}>{skillType}</color>";
        _currentAttackTypeAndTypeText = null;
    }
    
    private void PreviewSkillInit(SkillInventoryItem skillInventoryItem)
    {
        SkillItemSO currentSkillData = skillInventoryItem.data as SkillItemSO;
        
        if (!_skillTypes.ContainsKey(currentSkillData.reflectionName))
            _skillTypes.Add(currentSkillData.reflectionName, Type.GetType(currentSkillData.reflectionName));
            
        if(_currentPreviewSkill != null)
            Destroy(_currentPreviewSkill.gameObject);
            
        _currentPreviewSkill = Instantiate
            (SkillManager.Instance.GetSkill(_skillTypes[currentSkillData.reflectionName]), transform);
        
        var nodeInitEvt = SkillNodeEvents.SkillNodeInitEvent;
        nodeInitEvt.skillInventoryItem = skillInventoryItem;
        nodeInitEvt.skill = _currentPreviewSkill;
        _skillNodeEventChannelSO.RaiseEvent(nodeInitEvt); 
    }
}
