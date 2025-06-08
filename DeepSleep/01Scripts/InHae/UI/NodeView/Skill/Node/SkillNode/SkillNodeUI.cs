using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.NodeEvent.PartNodeEvents;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.EventSystem.SoundEvent;
using IH.Manager;
using IH.UI;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;

public class SkillNodeUI : BaseNode
{
    [SerializeField] private PartNodeUI _partNodeUI;
    [SerializeField] private SpecialPartNodeUI _specialPartNodeUI;
    
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private GameEventChannelSO _partNodeEventChannel;
    [SerializeField] private GameEventChannelSO _spsecialPartNodeEventChannel;
    [SerializeField] private GameEventChannelSO _soundChannelSO;
    [SerializeField] private SoundSO _equipPartSound;

    public SkillInventoryItem currentSkillItem;
    
    private Dictionary<Vector2Int, PartNodeUI> _allPartNodeUIDictionary = new ();
    private List<PartNodeUI> _connectPartNodeUI = new List<PartNodeUI>();

    private Dictionary<PartType, PartNodeUI> _partTypeCheckDictionary = new Dictionary<PartType, PartNodeUI>();
    
    private Transform _nodeParent;
    
    private float _nodeOffset;

    protected override void Awake()
    {
        base.Awake();
        _partNodeEventChannel.AddListener<AutoEquipPartEvent>(HandleAutoEquipPartEvent);
        _spsecialPartNodeEventChannel.AddListener<AutoEquipAbilityEvent>(HandleAutoEquipAbilityEvent);
    }

    private void OnDestroy()
    {
        _partNodeEventChannel.RemoveListener<AutoEquipPartEvent>(HandleAutoEquipPartEvent);
        _spsecialPartNodeEventChannel.RemoveListener<AutoEquipAbilityEvent>(HandleAutoEquipAbilityEvent);
    }
    
    private void HandleAutoEquipAbilityEvent(AutoEquipAbilityEvent evt)
    {
        List<SpecialPartNodeUI> specialNodeUIList = _allPartNodeUIDictionary.Values
            .OfType<SpecialPartNodeUI>()
            .Where(x=> x.isAbilityEmpty).OrderBy(x => x.index).ToList();
        
        if(specialNodeUIList.Count == 0)
            return;
        
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _equipPartSound;
        soundPlayEvt.position = transform.position;
        _soundChannelSO.RaiseEvent(soundPlayEvt);
        
        specialNodeUIList[0].NodeAbilityChange(evt.ability);
        InventoryManager.Instance.RemoveInventoryItemWithSo(evt.ability.data);
        SkillNodeUpdate();
    }
    
    private void HandleAutoEquipPartEvent(AutoEquipPartEvent evt)
    {
        List<PartNodeUI> partNodeUIList = _allPartNodeUIDictionary.Values
            .Where(x => x.isPartEmpty).OrderBy(x => x.index).ToList();
        
        if(partNodeUIList.Count==0 || partNodeUIList[0].isSpecialMode)
            return;
        
        NodeEquipData nodeEquipData = new NodeEquipData
        {
            partInventoryItem = evt.part
        };
        
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _equipPartSound;
        soundPlayEvt.position = transform.position;
        _soundChannelSO.RaiseEvent(soundPlayEvt);
        
        partNodeUIList[0].UpdateNode(nodeEquipData);
        InventoryManager.Instance.RemoveInventoryItemWithSo(evt.part.data);
        SkillNodeUpdate();
    }

    public void Init(SkillInventoryItem currentSkillInventoryItem, Skill skill)
    {
        _nodeOffset = NodeModular.NodeOffset;
        
        currentSkillItem = currentSkillInventoryItem;
        currentSkill = skill;

        _nodeParent = transform.parent;
        
        image.sprite = currentSkillInventoryItem.data.icon;
        image.color = Color.white;

        NodeGenerate();
        AddConnectAbleNode();
    }
    
    private void UpdatePart()
    {
        currentSkill.DataInit();
        currentSkill.UseActionClear();
        
        foreach (var part in _connectPartNodeUI)
            part.InitCurrentPart();
        
        for (int i = 0; i < _connectPartNodeUI.Count; i++)
            CheckContainPart(_connectPartNodeUI[i]);

        foreach (PartNodeUI partNodeUI in _partTypeCheckDictionary.Values)
        {
            if(partNodeUI ==null)
                continue;
            
            partNodeUI.EquipCurrentSkill();
        }

        var evt = SkillNodeEvents.SkillStatViewInitEvent;
        evt.skillInventoryItem = currentSkillItem;
        evt.skill = currentSkill;
        _skillNodeEventChannel.RaiseEvent(evt);
    }

    private void CheckContainPart(PartNodeUI partNodeUI)
    {
        PartType type = partNodeUI.CurrentEquipData.partInventoryItem.partNode.partType;
        if (type == PartType.Default)
        {
            partNodeUI.EquipCurrentSkill();
            return;
        }

        if (_partTypeCheckDictionary[type] == null)
        {
            _partTypeCheckDictionary[type] = partNodeUI;
            return;
        }

        if (!_partTypeCheckDictionary.ContainsKey(type)) 
            return;
        
        if (_partTypeCheckDictionary[type].index < partNodeUI.index)
        {
            _partTypeCheckDictionary[type].DisablePart();
            _partTypeCheckDictionary[type] = partNodeUI;
        }
        else
        {
            partNodeUI.DisablePart();
        }
    }

    public void ConnectNode(PartNodeUI partNodeUI)
    {
        _connectPartNodeUI.Add(partNodeUI);
    }

    private void NodeGenerate()
    {
        foreach (var nodeData in currentSkillItem.nodeGridDictionary)
        {
            NodeData node = nodeData.Value;
            
            PartNodeUI currentPartNode;
            if(node.isSpecial)
                currentPartNode = Instantiate(_specialPartNodeUI, _nodeParent);
            else
                currentPartNode = Instantiate(_partNodeUI, _nodeParent);
                
            currentPartNode.transform.SetAsLastSibling();
            currentPartNode.transform.localPosition =
                new Vector2(node.grid.x * 0.5f * _nodeOffset, node.grid.y * _nodeOffset);
            
            _allPartNodeUIDictionary.Add(nodeData.Key, currentPartNode);
            
            currentPartNode.Init(this, nodeData.Value);
        }
    }

    private void AddConnectAbleNode()
    {
        foreach (var nodeUI in _allPartNodeUIDictionary)
        {
            NodeData currentData = nodeUI.Value.CurrentNodeData;
            
            foreach (var connectNodeIndex in currentData.connectNodeGridList)
                nodeUI.Value.connectedNodes.Add(_allPartNodeUIDictionary[connectNodeIndex]);
                
            nodeUI.Value.LineConnect();
        }

        for (int i = 0; i < 6; i++)
        {
            Vector2Int closeGrid = Vector2Int.zero;
            closeGrid += NodeModular.GetNodeDirGrid((NodeDir)i);
            connectedNodes.Add(_allPartNodeUIDictionary[closeGrid]);
        }
        
        LineConnect();
    }
    
    public void SkillNodeUpdate()
    {
        DisableAllLines();
        foreach (PartNodeUI node in _connectPartNodeUI)
        {
            node.DisableAllLines();
            node.UnEquipCurrentPart();
            node.isSkillConnected = false;
        }
        
        TypeDictionaryInit();
        _connectPartNodeUI.Clear();
        
        NodeConnectCheck();
        UpdatePart();
    }

    protected override IEnumerator WaitLineConnect()
    {
        yield return base.WaitLineConnect();
        SkillNodeUpdate();
    }

    private void TypeDictionaryInit()
    {
        foreach (var type in Enum.GetValues(typeof(PartType)))
        {
            if((PartType)type == PartType.Default)
                continue;

            _partTypeCheckDictionary[(PartType)type] = null;
        }
    }
}
