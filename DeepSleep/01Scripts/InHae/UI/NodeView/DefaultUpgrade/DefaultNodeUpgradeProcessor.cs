using System;
using DG.Tweening;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.EventSystem.SoundEvent;
using IH.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using YH.EventSystem;

public class DefaultNodeUpgradeProcessor : MonoBehaviour, IDefaultNodeUpgradeCompo, IPointerClickHandler
{
    [FormerlySerializedAs("_upgradePreviewNode")]
    [Header("Assign Inspector Instance")]
    [SerializeField] private DefaultUpgradePreviewNode defaultUpgradePreviewNode;
    
    [SerializeField] private GameEventChannelSO _soundChannelSO;
    [SerializeField] private SoundSO _defaultUpgradeSound;
    
    private DefaultUpgradePartNode _selectedNode;
    private bool _isSelected;
    
    private NodeData _selectedNodeData => _selectedNode.CurrentNodeData;
    private NodeDir _selectedDir;
    
    private float _nodeOffset;

    private DefaultNodeUpgradeManager _manager;
    private DefaultNodeUpgradeView _view;
    
    private Transform _nodeParent;
    private SkillInventoryItem _selectedSkill;

    private bool _canUpgrade;

    public event Action UpgradeAction;
    
    public void Initialize(DefaultNodeUpgradeManager manager)
    {
        _manager = manager;
        _view = _manager.GetCompo<DefaultNodeUpgradeView>();
        
        _selectedNode = null;
        
        _manager.defaultNodeEventChannel.AddListener<UpgradeNodeSelectEvent>(HandleNodeUpgradeSelectEvent);
        _manager.defaultNodeEventChannel.AddListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.defaultNodeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
    }
    
    private void OnDestroy()
    {
        _manager.defaultNodeEventChannel.RemoveListener<UpgradeNodeSelectEvent>(HandleNodeUpgradeSelectEvent);
        _manager.defaultNodeEventChannel.RemoveListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.defaultNodeEventChannel.RemoveListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
    }
    
    private void Awake()
    {
        _nodeOffset = NodeModular.NodeOffset;
    }

    private void FixedUpdate()
    {
        if(!_isSelected)
            return;

        SetDir();
        SetPreviewNode();
    }
    
    private void HandleNodeUpgradeSelectEvent(UpgradeNodeSelectEvent evt)
    {
        if (!_canUpgrade)
            return;
        
        _isSelected = false;
        _selectedNode = evt.selectNode;
        defaultUpgradePreviewNode.gameObject.SetActive(false);
        
        if (evt.isSelected == false)
            return;

        Vector3 pos = _selectedNode.transform.localPosition * (-1 * _nodeParent.localScale.x);
        float time = 0.5f;
        
        float distance = Vector2.Distance(pos, _nodeParent.localPosition);
        if (distance == 0f)
            time = 0;
        
        _nodeParent.DOLocalMove(pos, time).SetUpdate(true).OnComplete(()=>
        {
            _isSelected = true;
            defaultUpgradePreviewNode.gameObject.SetActive(true);
            defaultUpgradePreviewNode.Init(_selectedNode);
        });
    }

    private void HandleUpgradeSkillSelectEvent(UpgradeSkillSelectEvent evt) => _selectedSkill = evt.item;
    
    private void HandleNodeParentInitEvent(NodeParentInitEvent evt) => _nodeParent = evt.parent;

    public void SetCanUpgradeEnd(bool canUpgradeEnd) => _canUpgrade = canUpgradeEnd;
    
    private void SetDir()
    {
        Vector2 initVector = Vector2.up;
        Vector2 mouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(_selectedNode.transform.position);
        float dot = Vector2.Dot(mouseDir.normalized, initVector);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        
        Vector3 cross = Vector3.Cross(initVector, mouseDir);
        if (cross.z > 0)
            angle = 360 - angle;

        int nodeDir = (int)angle / 60 + 2;
        if (nodeDir >= 6)
            nodeDir -= 6;
        
        _selectedDir = (NodeDir)nodeDir;
    }

    private void SetPreviewNode()
    {
        Vector2Int upgradeGrid = _selectedNodeData.grid + NodeModular.GetNodeDirGrid(_selectedDir);
        if (_selectedNodeData.connectNodeGridList.Contains(upgradeGrid) || upgradeGrid == Vector2Int.zero)
        {
            defaultUpgradePreviewNode.Disable();
            return;
        }
        
        defaultUpgradePreviewNode.transform.localScale = _nodeParent.localScale;

        Vector2Int previewGrid = NodeModular.GetNodeDirGrid(_selectedDir);
        Vector2 pos = new Vector2(previewGrid.x * 0.5f * _nodeOffset, previewGrid.y * _nodeOffset);
        pos *= _nodeParent.localScale.x;
        
        defaultUpgradePreviewNode.transform.localPosition = pos;
        _nodeParent.transform.localPosition = _selectedNode.transform.localPosition * (-1 * _nodeParent.localScale.x);
        
        if (_selectedSkill.nodeGridDictionary.ContainsKey(upgradeGrid))
            defaultUpgradePreviewNode.NewNodeInit(false);
        else
            defaultUpgradePreviewNode.NewNodeInit(true);
    }

    private void NodeUpgrade()
    {
        Vector2Int upgradeGrid = _selectedNodeData.grid + NodeModular.GetNodeDirGrid(_selectedDir);
        if(_selectedNodeData.connectNodeGridList.Contains(upgradeGrid) || upgradeGrid == Vector2Int.zero)
            return;

        NodeData upgradeNodeData;
        if (!_selectedSkill.nodeGridDictionary.TryGetValue(upgradeGrid, out var data))
        {
            upgradeNodeData = new NodeData(false, _selectedSkill.nodeGridDictionary.Count, 1, upgradeGrid);
            _selectedSkill.nodeGridDictionary.Add(upgradeGrid, upgradeNodeData);
            _view.AddNode(_selectedNode, upgradeNodeData);
        }
        else
        {
            upgradeNodeData = data;
            _view.ReConnectNode(_selectedNode.CurrentNodeData.grid, upgradeNodeData.grid);
        }
            
        _selectedNodeData.connectNodeGridList.Add(upgradeGrid);
        upgradeNodeData.connectNodeGridList.Add(_selectedNodeData.grid);
        
        _isSelected = false;
        defaultUpgradePreviewNode.gameObject.SetActive(false);
        
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _defaultUpgradeSound;
        soundPlayEvt.position = transform.position;
        _soundChannelSO.RaiseEvent(soundPlayEvt);
        
        UpgradeAction?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_isSelected || eventData.button != PointerEventData.InputButton.Left)
            return;
        NodeUpgrade();
    }
}
