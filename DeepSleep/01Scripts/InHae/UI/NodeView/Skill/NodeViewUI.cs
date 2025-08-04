using System.Collections.Generic;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;

public class NodeViewUI : MonoBehaviour
{
    [SerializeField] private WindowPanel _nodeUIWindow;
    
    [FormerlySerializedAs("_nodeEventChannel")]
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private GameEventChannelSO _nodeChainEventChannel;
    
    [SerializeField] private RectTransform _nodeRowParent;
    [SerializeField] private SkillNodeUI _baseSkillNodeUI;
    
    private Skill _currentSkill;
    private List<BaseNode> _currentNodes = new ();
    
    private void Awake()
    {
        _skillNodeEventChannel.AddListener<SkillNodeInitEvent>(HandleInitNodeSkillEvent);
        _skillNodeEventChannel.AddListener<SkillUnEquipCheckEvent>(HandleUnEquipCheck);
    }

    private void OnDestroy()
    {
        _skillNodeEventChannel.RemoveListener<SkillNodeInitEvent>(HandleInitNodeSkillEvent);
        _skillNodeEventChannel.RemoveListener<SkillUnEquipCheckEvent>(HandleUnEquipCheck);
    }
    
    private void HandleUnEquipCheck(SkillUnEquipCheckEvent evt)
    {
        if (_currentSkill == null || _currentSkill != evt.skill)
            return;
        
        _nodeUIWindow.HandleCloseUI();
    }

    private void HandleInitNodeSkillEvent(SkillNodeInitEvent evt)
    {
        foreach (var node in _currentNodes)
            PoolManager.Instance.Push(node, true);
        
        _currentNodes.Clear();
        
        _currentSkill = evt.skill;
        SkillInventoryItem currentSkillInventoryItem = evt.skillInventoryItem;

        SkillNodeUI currentSkillNodeUI = PoolManager.Instance.Pop(NodeUIPoolingType.SkillNode) as SkillNodeUI;
        currentSkillNodeUI.transform.SetParent(_nodeRowParent);
        currentSkillNodeUI.transform.localPosition = Vector3.zero;
        currentSkillNodeUI.transform.localScale = Vector3.one;
        
        currentSkillNodeUI.Init(currentSkillInventoryItem, _currentSkill, _currentNodes);
    }
}