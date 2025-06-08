using System;
using System.Collections;
using System.Collections.Generic;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.UI;
using UnityEngine;

public class DefaultNodeUpgradeView : MonoBehaviour, IDefaultNodeUpgradeCompo
{
    [SerializeField] private DefaultUpgradePartNode _defaultUpgradePartNode;
    [SerializeField] private DefaultUpgradePartNode _defaultUpgradeSpecialPartNode;
    [SerializeField] private DefaultUpgradeSkillNode _defaultUpgradeSkillNode;
    private DefaultUpgradeSkillNode _skillNode;
    
    private Dictionary<Vector2Int, DefaultUpgradePartNode> _nodeUIDictionary = new ();
    
    private float _nodeOffset;
    
    private DefaultNodeUpgradeManager _manager;
    private SkillInventoryItem _selectedItem;
    private Transform _nodeParent;
    
    public event Action UpgradeEndAction;

    public void Initialize(DefaultNodeUpgradeManager manager)
    {
        _manager = manager;
        _manager.defaultNodeEventChannel.AddListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.defaultNodeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
    }
    
    private void OnDestroy()
    {
        _manager.defaultNodeEventChannel.RemoveListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.defaultNodeEventChannel.RemoveListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
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
        DefaultUpgradeSkillNode skillNode = Instantiate(_defaultUpgradeSkillNode, _nodeParent);
        skillNode.SkillNodeInit(_selectedItem);
        _skillNode = skillNode;
        
        foreach (var nodeData in _selectedItem.nodeGridDictionary)
        {
            NodeData node = nodeData.Value;
            
            DefaultUpgradePartNode currentPartNode;
            if(node.isSpecial)
                currentPartNode = Instantiate(_defaultUpgradeSpecialPartNode, _nodeParent);
            else
                currentPartNode = Instantiate(_defaultUpgradePartNode, _nodeParent);
            
            currentPartNode.transform.SetAsLastSibling();
            currentPartNode.transform.localPosition =
                new Vector2(node.grid.x * 0.5f * _nodeOffset, node.grid.y * _nodeOffset);
            
            _nodeUIDictionary.Add(nodeData.Key,currentPartNode);
            
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

    public void AddNode(DefaultUpgradePartNode baseNode, NodeData newNodeData)
    {
        DefaultUpgradePartNode partNode = Instantiate(_defaultUpgradePartNode, _nodeParent);
        Vector2Int grid = newNodeData.grid;
        
        partNode.transform.SetAsLastSibling();
        partNode.transform.localPosition = new Vector2(grid.x * 0.5f * _nodeOffset, grid.y * _nodeOffset);
        partNode.Init(newNodeData, null);
        
        _nodeUIDictionary.Add(grid, partNode);
        
        baseNode.connectedNodes.Add(partNode);
        baseNode.LineConnect();

        StartCoroutine(EndCheck());
    }

    public void ReConnectNode(Vector2Int baseNodeGrid, Vector2Int newNodeGrid)
    {
        DefaultUpgradePartNode basePartNode = _nodeUIDictionary[baseNodeGrid];
        DefaultUpgradePartNode newPartNode = _nodeUIDictionary[newNodeGrid];
        
        newPartNode.connectedNodes.Add(basePartNode);
        newPartNode.LineConnect();
        
        basePartNode.connectedNodes.Add(newPartNode);
        basePartNode.LineConnect();
        
        StartCoroutine(EndCheck());
    }

    private IEnumerator EndCheck()
    {
        yield return new WaitForSeconds(0.5f);
        UpgradeEndAction?.Invoke();
    }
}