using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class NodeUpgradeSkillInfoUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _skillImage;

    private void Awake()
    {
        _uiEventChannelSO.AddListener<UpgradeSelectSkillEvent>(HandleNodeUpgradeSkillInfo);        
    }

    private void OnDestroy()
    {
        _uiEventChannelSO.RemoveListener<UpgradeSelectSkillEvent>(HandleNodeUpgradeSkillInfo);        
    }
    
    private void HandleNodeUpgradeSkillInfo(UpgradeSelectSkillEvent evt)
    {
        SkillItemSO skillItemSO = evt.item.data as SkillItemSO;
        
        _title.SetText(skillItemSO.itemName);
        _description.SetText(skillItemSO.itemDescription);
    }
}
