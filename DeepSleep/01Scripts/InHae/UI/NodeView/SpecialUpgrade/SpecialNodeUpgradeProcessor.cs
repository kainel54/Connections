using System;
using DG.Tweening;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;

public class SpecialNodeUpgradeProcessor : MonoBehaviour, ISpecialNodeUpgradeCompo
{
    [SerializeField] private GameEventChannelSO _soundChannelSO;
    [SerializeField] private SoundSO _specialUpgradeSound;
    
    private SpecialUpgradePartNode _selectedNode;
    private bool _isSelected;
    
    private NodeData _selectedNodeData => _selectedNode.CurrentNodeData;
    
    private SpecialNodeUpgradeManager _manager;
    private SpecialNodeUpgradeView _view;
    
    private Transform _nodeParent;
    private SkillInventoryItem _selectedSkill;

    private bool _canUpgrade;

    public event Action UpgradeAction;
    
    public void Initialize(SpecialNodeUpgradeManager manager)
    {
        _manager = manager;
        _view = _manager.GetCompo<SpecialNodeUpgradeView>();
        
        _manager.specialNodeUpgradeEventChannel.AddListener<UpgradeNodeSelectEvent>(HandleNodeUpgradeSelectEvent);
        _manager.specialNodeUpgradeEventChannel.AddListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.specialNodeUpgradeEventChannel.AddListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
    }
    
    private void OnDestroy()
    {
        _manager.specialNodeUpgradeEventChannel.RemoveListener<UpgradeNodeSelectEvent>(HandleNodeUpgradeSelectEvent);
        _manager.specialNodeUpgradeEventChannel.RemoveListener<NodeParentInitEvent>(HandleNodeParentInitEvent);
        _manager.specialNodeUpgradeEventChannel.RemoveListener<UpgradeSkillSelectEvent>(HandleUpgradeSkillSelectEvent);
    }

    private void HandleNodeUpgradeSelectEvent(UpgradeNodeSelectEvent evt)
    {
        if(!_canUpgrade)
            return;
        
        _isSelected = false;
        _selectedNode = evt.selectNode;
        
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
            NodeUpgrade();
        });
    }

    private void HandleUpgradeSkillSelectEvent(UpgradeSkillSelectEvent evt) => _selectedSkill = evt.item;
    private void HandleNodeParentInitEvent(NodeParentInitEvent evt) => _nodeParent = evt.parent;
    
    public void SetCanUpgradeEnd(bool canUpgradeEnd) => _canUpgrade = canUpgradeEnd;

    private void NodeUpgrade()
    {
        _view.NodeChange(_selectedNodeData.grid);
        
        NodeData currentData = _selectedSkill.nodeGridDictionary[_selectedNodeData.grid];
        currentData.isSpecial = true;
        _selectedSkill.nodeGridDictionary[_selectedNodeData.grid] = currentData;
        
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _specialUpgradeSound;
        soundPlayEvt.position = transform.position;
        _soundChannelSO.RaiseEvent(soundPlayEvt);
        
        SetCanUpgradeEnd(false);
    }
}
