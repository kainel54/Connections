using System;
using System.Collections.Generic;
using IH.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartNodeUI : BaseNode, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SkillNodeUI skillNode;
    
    [HideInInspector] public int index;
    [HideInInspector] public bool isDisable;
    [HideInInspector] public bool isChainMode;
    [HideInInspector] public bool isSkillConnected;
    [HideInInspector] public bool isChained;

    private PartNodeChainCheck _chainCheck;
    public event Action ReturnInventoryEvent;
    
    private PartNode _currentNode => isChained ? _chainCheck.chainPartNode : CurrentData.partInventoryItem.partNode;
    public NodeData CurrentData { get; private set; }
    public bool isEmpty => CurrentData?.partInventoryItem == null || CurrentData?.partInventoryItem.data == null;

    private RectTransform _dragTarget;
    protected override void Awake()
    {
        base.Awake();
        _chainCheck = GetComponent<PartNodeChainCheck>();
    }

    public void Init(SkillNodeUI skillNodeUI, int idx, NodeData nodeData)
    {
        skillNode = skillNodeUI;
        currentSkill = skillNode.currentSkill;
        
        gameObject.SetActive(true);
        index = idx;
        
        NodeDataInit();
        UpdateNode(nodeData);
    }

    private void VariableInit()
    {
        isChained = false;
        isDisable = false;
    }

    public void UpdateNode(NodeData nodeData)
    {
        skillNode.currentSkillItem.equipNodeData[index] = nodeData;
        CurrentData = nodeData;
        
        if (CurrentData != null && CurrentData.partInventoryItem.data != null)
        {
            _chainCheck.Init();
            
            _image.sprite = CurrentData.partInventoryItem.data.icon;
            _image.color = Color.white;
        }
        else
        {
            CleanUpSlot();
        }
    }

    private void NodeDataInit()
    {
        if (!skillNode.currentSkillItem.equipNodeData.ContainsKey(index))
        {
            skillNode.currentSkillItem.equipNodeData[index] = new NodeData
            {
                partInventoryItem = null,
                chainList = new List<PartInventoryItem>()
            };
        }
    }
    
    public void SaveChainList(List<PartInventoryItem> partInventoryItems)
    {
        skillNode.currentSkillItem.equipNodeData[index].chainList = partInventoryItems;
    }

    private void CleanUpSlot()
    {
        _chainCheck.CleanUp();
        
        skillNode.currentSkillItem.equipNodeData[index] = null;
        CurrentData = null;
        
        _image.sprite = null;
        _image.color = Color.clear;
    }
    
    public void EquipCurrentSkill()
    {
        if(isEmpty)
            return;
        
        if(isDisable)
            EnablePart();
        
        _currentNode.EquipPart(currentSkill);
    }

    public void UnEquipCurrentPart()
    {
        if(isEmpty || !isSkillConnected)
            return;
        
        _currentNode.UnEquipPart(currentSkill);
    }
    
    public void InitCurrentPart()
    {
        if(isEmpty || !isSkillConnected)
            return;

        _currentNode.InitPart(currentSkill);
    }
    
    public void DisablePart()
    {
        isDisable = true;
        _image.color = Color.gray;
        UnEquipCurrentPart();
    }

    private void EnablePart()
    {
        isDisable = false;
        _image.color = Color.white;
    }

    public void ReturnInventoryItem()
    {
        if(isEmpty)
            return;
        
        if(isSkillConnected)
            UnEquipCurrentPart();
        
        InventoryManager.Instance.AddInventoryItemWithSo(CurrentData.partInventoryItem.data);
        ReturnInventoryEvent?.Invoke();
        
        skillNode.currentSkillItem.equipNodeData[index] = null;
        CurrentData = null;
        
        VariableInit();
        
        if (isSkillConnected)
            skillNode.StartNodeCheck();
        
        _image.color = Color.clear;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left || isEmpty)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        dragItem.StartDrag(CurrentData.partInventoryItem);
        _dragTarget = dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;
        
        _image.color = Color.clear;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        dragItem.EndDrag();
        
        if (isEmpty || dragItem.successDrop)
            return;
        
        _image.sprite = CurrentData.partInventoryItem.data.icon;
        _image.color = Color.white;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left || isEmpty)
            return;
        _dragTarget.position = Input.mousePosition;
    }
}
