using System;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YH.EventSystem;

public class SkillInfoPanelUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _nodeEventChannel;
    
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;

    [SerializeField] private Transform _statTextParent;
    private Dictionary<SkillFieldDataType, SkillStatTextUI> _statTextUI = new Dictionary<SkillFieldDataType, SkillStatTextUI>();

    private void Awake()
    {
        _nodeEventChannel.AddListener<SkillStatViewInitEvent>(HandleSkillStatViewInit);

        _statTextParent.GetComponentsInChildren<SkillStatTextUI>().ToList()
            .ForEach(x => _statTextUI.Add(x.fieldType, x));
    }

    private void OnDestroy()
    {
        _nodeEventChannel.RemoveListener<SkillStatViewInitEvent>(HandleSkillStatViewInit);
    }

    private void HandleSkillStatViewInit(SkillStatViewInitEvent evt)
    {
        _skillIcon.sprite = evt.skillInventoryItem.data.icon;
        _skillName.text = evt.skillInventoryItem.data.itemName;
        _skillDescription.text = evt.skillInventoryItem.data.itemDescription;

        foreach (var statTextUI in _statTextUI)
            statTextUI.Value.Init(evt.skill.GetSkillData(statTextUI.Key));
    }
}
