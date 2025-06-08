using System;
using System.Collections;
using System.Collections.Generic;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.UI;
using UnityEngine;

public class SpecialNodeUpgradeView : MonoBehaviour, ISpecialNodeUpgradeCompo
{
    [SerializeField] private SpecialUpgradePartNode _specialUpgradePartNode;
    [SerializeField] private SpecialUpgradePartNode _specialUpgradeSpecialPartNode;
    [SerializeField] private SpecialUpgradeSkillNode _specialUpgradeSkillNode;
    private SpecialUpgradeSkillNode _skillNode;
    
    private Dictionary<Vector2Int, SpecialUpgradePartNode> _nodeUIDictionary = new ();
    
    private float _nodeOffset;
    
    private SpecialNodeUpgradeManager _manager;
    private SkillInventoryItem _selectedItem;
    private Transform _nodeParent;
    
    public event Action UpgradeEndAction;
    
    public void Initialize(SpecialNodeUpgradeManager manager)
    {
        _manager = manager;
        _manager.specialNodeUpgradeEventChannel.AddListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.specialNodeUpgradeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
    }

    private void OnDestroy()
    {
        _manager.specialNodeUpgradeEventChannel.RemoveListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.specialNodeUpgradeEventChannel.RemoveListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
    }
    
    private void Awake()
    {
        _nodeOffset = NodeModular.NodeOffset;
    }

    private void HandleNodeParentInitEvent(NodeParentInitEvent evt) => _nodeParent = evt.parent;
    private void HandleUpgradeSkillSelectEvent(UpgradeSkillSelectEvent evt) => _selectedItem = evt.item;

    private void AddConnectAbleNode()
    {
        foreach (var nodeUI in _nodeUIDictionary)
        {
            NodeData currentData = nodeUI.Value.CurrentNodeData;
            foreach (var connectNodeIndex in currentData.connectNodeGridList)
                nodeUI.Value.connectedNodes.Add(_nodeUIDictionary[connectNodeIndex]);
            nodeUI.Value.LineConnect();
        }
        
        for (int i = 0; i < 6; i++)
        {
            Vector2Int closeGrid = Vector2Int.zero;
            closeGrid += NodeModular.GetNodeDirGrid((NodeDir)i);
            _skillNode.connectedNodes.Add(_nodeUIDictionary[closeGrid]);
        }
        
        _skillNode.LineConnect();
    }

    private void NodeInit()
    {
        SpecialUpgradeSkillNode skillNode = Instantiate(_specialUpgradeSkillNode, _nodeParent);
        skillNode.SkillNodeInit(_selectedItem);
        _skillNode = skillNode;
        
        foreach (var nodeData in _selectedItem.nodeGridDictionary)
        {
            NodeData node = nodeData.Value;
            
            SpecialUpgradePartNode currentPartNode;
            if(node.isSpecial)
                currentPartNode = Instantiate(_specialUpgradeSpecialPartNode, _nodeParent);
            else
                currentPartNode = Instantiate(_specialUpgradePartNode, _nodeParent);
            
            currentPartNode.transform.SetAsLastSibling();
            currentPartNode.transform.localPosition =
                new Vector2(node.grid.x * 0.5f * _nodeOffset, node.grid.y * _nodeOffset);
            
            _nodeUIDictionary.Add(nodeData.Key, currentPartNode);
            
            NodeEquipData nodeEquipData = null;
            if (_selectedItem.equipNodeData.TryGetValue(nodeData.Value.index, out var part))
                nodeEquipData = part;

            currentPartNode.Init(node, nodeEquipData);
        }
    }

    public void CreateNodes()
    {
        _nodeUIDictionary.Clear();
        
        for (int i = 0; i < _nodeParent.childCount; i++)
            Destroy(_nodeParent.GetChild(i).gameObject);
        
        NodeInit();
        AddConnectAbleNode();
    }

    public void NodeChange(Vector2Int currentGrid)
    {
        _nodeUIDictionary[currentGrid].ChangeColor(0.5f);
        StartCoroutine(EndCheck());
    }
    
    private IEnumerator EndCheck()
    {
        yield return new WaitForSeconds(1.25f);
        UpgradeEndAction?.Invoke();
    }
}
