using System.Collections.Generic;
using System.Linq;
using IH.EventSystem;
using UnityEngine;
using YH.EventSystem;

public class NodeViewUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _nodeEventChannel;
    
    [SerializeField] private RectTransform _nodeRowParent;
    [SerializeField] private SkillNodeUI _baseSkillNodeUI;
    [SerializeField] private PartNodeUI _partNodeUI;
    [SerializeField] private PartRow _partRow;
    
    [SerializeField] private ChainButton _chainButton;
    private bool _isChainMode;
    
    private void Awake()
    {
        _nodeEventChannel.AddListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
        _nodeEventChannel.AddListener<NodeUpgradeEvent>(HandleNodeUpgradeEvent);
    }

    private void OnDestroy()
    {
        _nodeEventChannel.RemoveListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
        _nodeEventChannel.RemoveListener<NodeUpgradeEvent>(HandleNodeUpgradeEvent);
    }
    
    private void HandleNodeUpgradeEvent(NodeUpgradeEvent evt)
    {
        evt.skillInventoryItem.rowAndNodeCountList.Add(evt.count);
    }

    private void HandleInitNodeSkillEvent(InitNodeSkillEvent evt)
    {
        for (int i = 0; i < _nodeRowParent.childCount; i++)
            Destroy(_nodeRowParent.GetChild(i).gameObject);
        
        Skill currentSkill = evt.skill;
        SkillInventoryItem currentSkillInventoryItem = evt.skillInventoryItem;

        PartRow partRow = Instantiate(_partRow, _nodeRowParent);
        
        SkillNodeUI currentSkillNodeUI = Instantiate(_baseSkillNodeUI, partRow.transform);
        currentSkillNodeUI.Init(currentSkillInventoryItem, currentSkill, _nodeRowParent, _nodeEventChannel);
    }
    
    public void ChainButtonClicked()
    {
        // 이미 체인 모드인데 버튼을 눌렀을 경우 -> ( 취소 )
        ChangeChainMode(!_isChainMode);
        _isChainMode = !_isChainMode;
    }

    public void ChangeChainMode(bool isActive)
    {
        _chainButton.ChangeText(isActive);
        var evt = NodeEvents.ChainModeChangeEvent;
        evt.isActive = isActive;
        _nodeEventChannel.RaiseEvent(evt);
    }

    public void VariableInit() => _isChainMode = false;
}