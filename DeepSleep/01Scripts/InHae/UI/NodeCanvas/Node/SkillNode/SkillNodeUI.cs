using System;
using System.Collections;
using System.Collections.Generic;
using IH.EventSystem;
using Unity.VisualScripting;
using UnityEngine;
using YH.EventSystem;

public class SkillNodeUI : BaseNode
{
    [SerializeField] private PartRow _partRow;
    
    public SkillInventoryItem currentSkillItem;
    
    private List<PartNodeUI> _connectPartNodeUI = new List<PartNodeUI>();

    private Dictionary<PartType, PartNodeUI> _partTypeCheckDictionary = new Dictionary<PartType, PartNodeUI>();
    
    private GameEventChannelSO _nodeEventChannel;
    private Transform _nodeRowParent;
    private List<PartRow> _partRows;
    
    protected override void Awake()
    {
        base.Awake();
        _partRows = new List<PartRow>();
    }

    public void Init(SkillInventoryItem currentSkillInventoryItem, Skill skill, 
        Transform nodeParent, GameEventChannelSO nodeEventChannel)
    {
        _nodeEventChannel = nodeEventChannel;
        _partRows.Clear();
        
        currentSkillItem = currentSkillInventoryItem;
        currentSkill = skill;

        _nodeRowParent = nodeParent;
        
        _image.sprite = currentSkillInventoryItem.data.icon;
        _image.color = Color.white;

        NodeGenerate();
        AddConnectAbleNode();
    }
    
    private void UpdatePart()
    {
        currentSkill.DataInit();
        foreach (var part in _connectPartNodeUI)
            part.InitCurrentPart();
        
        for (int i = 0; i < _connectPartNodeUI.Count; i++)
        {
            CheckContainPart(_connectPartNodeUI[i]);
        }

        foreach (PartNodeUI partNodeUI in _partTypeCheckDictionary.Values)
        {
            if(partNodeUI ==null)
                continue;
            
            partNodeUI.EquipCurrentSkill();
        }

        var evt = NodeEvents.SkillStatViewInitEvent;
        evt.skillInventoryItem = currentSkillItem;
        evt.skill = currentSkill;
        _nodeEventChannel.RaiseEvent(evt);
    }

    private void CheckContainPart(PartNodeUI partNodeUI)
    {
        PartType type = partNodeUI.CurrentData.partInventoryItem.partNode.partType;
        if (type == PartType.Default)
        {
            partNodeUI.EquipCurrentSkill();
            return;
        }

        if (_partTypeCheckDictionary[type] == null)
            _partTypeCheckDictionary[type] = partNodeUI;

        if (!_partTypeCheckDictionary.ContainsKey(type)) 
            return;
        
        if (_partTypeCheckDictionary[type].index < partNodeUI.index)
        {
            _partTypeCheckDictionary[type].DisablePart();
            _partTypeCheckDictionary[type] = partNodeUI;
        }
        else
            partNodeUI.DisablePart();
    }

    public void ConnectNode(PartNodeUI partNodeUI)
    {
        _connectPartNodeUI.Add(partNodeUI);
    }

    private void NodeGenerate()
    {
        int idx = 0;
        List<int> rowAndNodeCountList = new List<int>(currentSkillItem.rowAndNodeCountList);
        
        for (int i = 0; i < rowAndNodeCountList.Count; i++)
        {
            // 나중에 풀링
            PartRow partRow = Instantiate(_partRow, _nodeRowParent);
            _partRows.Add(partRow);
            
            for (int j = 0; j < rowAndNodeCountList[i]; j++)
            {
                PartNodeUI currentPartNode;
                currentPartNode = partRow.partNodes[j];

                NodeData nodeData = null;
                
                if (currentSkillItem.equipNodeData.TryGetValue(idx, out var part))
                    nodeData = part;

                currentPartNode.Init(this, idx, nodeData);
                idx++;
            }
            partRow.SetPosition();
        }
    }

    private void AddConnectAbleNode()
    {
        // 나중에 무조건 수정 근데 지금은 이 방법 밖에 안 떠오름...
        // 열 개수
        for (int i = 0; i < _partRows.Count; i++)
        {
            if (i == _partRows.Count - 1)
                break;
            
            int connectCount = 0;
            // 그 열에 있는 칸 개수

            int currentRowCount = _partRows[i].activeNodeCount;
            int nextRowCount = _partRows[i + 1].activeNodeCount;
            
            for (int j = 0; j < currentRowCount; j++)
            {
                if (currentRowCount == 1)
                    connectCount = nextRowCount;
                else if (currentRowCount < nextRowCount)
                    connectCount = 2;
                else if (currentRowCount > 2 && nextRowCount == currentRowCount - 1)
                {
                    if (j == 0 || j == currentRowCount - 1)
                        connectCount = 1;
                    else
                        connectCount = 2;
                }
                else
                    connectCount = 1;

                // 현재 칸이 연결 가능한 노드 개수
                for (int k = 0; k < connectCount; k++) //현재 선택된 노드에 다음 열의 노드를 추가
                {
                    if (currentRowCount == nextRowCount)
                        _partRows[i].partNodes[j].connectedAbleNodes.Add(_partRows[i + 1].partNodes[j]);
                    else if (currentRowCount > nextRowCount )
                    {
                        _partRows[i].partNodes[j].connectedAbleNodes.Add(_partRows[i + 1]
                            .partNodes[Mathf.Clamp(j - 1 + k, 0, nextRowCount - 1)]);
                    }
                    else
                    {
                        if (currentRowCount == nextRowCount - 1 || currentRowCount == 1)
                            _partRows[i].partNodes[j].connectedAbleNodes.Add(_partRows[i + 1].partNodes[j + k]);
                        else
                            _partRows[i].partNodes[j].connectedAbleNodes.Add(_partRows[i + 1].partNodes[j * 2 + k]);
                    }
                }
                _partRows[i].partNodes[j].LineConnect();
            }
        }
        
        for (int i = 0; i < _partRows[0].activeNodeCount; i++)
            connectedAbleNodes.Add(_partRows[0].partNodes[i]);
        LineConnect();
    }
    
    public void StartNodeCheck()
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
        StartNodeCheck();
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
