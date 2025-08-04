using System;
using System.Collections;
using System.Collections.Generic;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.UI;
using ObjectPooling;
using UnityEngine;

public class DefaultNodeUpgradeView : MonoBehaviour, IDefaultNodeUpgradeCompo
{
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
        DefaultUpgradeSkillNode skillNode = PoolManager.Instance.Pop(NodeUIPoolingType.DefaultUpgradeSkillNode) 
            as DefaultUpgradeSkillNode;
        skillNode.transform.SetParent(_nodeParent);
        skillNode.transform.localPosition = Vector3.zero;
        skillNode.transform.localScale = Vector3.one;
        
        skillNode.SkillNodeInit(_selectedItem);
        _skillNode = skillNode;
        
        foreach (var nodeData in _selectedItem.nodeGridDictionary)
        {
            NodeData node = nodeData.Value;
            
            DefaultUpgradePartNode currentPartNode;
            if(node.isSpecial)
                currentPartNode = PoolManager.Instance.Pop(NodeUIPoolingType.DefaultUpgradeSpecialPartNode) 
                    as DefaultUpgradePartNode;
            else
                currentPartNode = PoolManager.Instance.Pop(NodeUIPoolingType.DefaultUpgradePartNode)
                    as DefaultUpgradePartNode;
            
            currentPartNode.transform.SetParent(_nodeParent);
            currentPartNode.transform.localScale = Vector3.one;
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
        if(_skillNode !=null)
            PoolManager.Instance.Push(_skillNode, true);
        foreach (var node in _nodeUIDictionary.Values)
            PoolManager.Instance.Push(node, true);
        
        _nodeUIDictionary.Clear();
        
        NodeInit();
        AddConnectAbleNode();
    }

    public void AddNode(DefaultUpgradePartNode baseNode, NodeData newNodeData)
    {
        DefaultUpgradePartNode partNode = PoolManager.Instance.Pop(NodeUIPoolingType.DefaultUpgradePartNode)
            as DefaultUpgradePartNode;
        
        partNode.transform.SetParent(_nodeParent);
        partNode.transform.localScale = Vector3.one;
        Vector2Int grid = newNodeData.grid;
        
        partNode.transform.SetAsLastSibling();
        partNode.transform.localPosition = new Vector2(grid.x * 0.5f * _nodeOffset, grid.y * _nodeOffset);
        partNode.Init(newNodeData, null);
        
        _nodeUIDictionary.Add(grid, partNode);
        
        baseNode.connectedNodes.Add(partNode);
        baseNode.LineConnectAndEnableCheck();

        StartCoroutine(EndCheck());
    }

    public void ReConnectNode(Vector2Int baseNodeGrid, Vector2Int newNodeGrid)
    {
        DefaultUpgradePartNode basePartNode = _nodeUIDictionary[baseNodeGrid];
        DefaultUpgradePartNode newPartNode = _nodeUIDictionary[newNodeGrid];
        
        newPartNode.connectedNodes.Add(basePartNode);
        newPartNode.LineConnectAndEnableCheck();
        
        basePartNode.connectedNodes.Add(newPartNode);
        basePartNode.LineConnectAndEnableCheck();
        
        StartCoroutine(EndCheck());
    }

    private IEnumerator EndCheck()
    {
        yield return new WaitForSeconds(0.5f);
        UpgradeEndAction?.Invoke();
    }
}