using System;
using System.Collections.Generic;
using IH.Manager;
using ObjectPooling;
using UnityEngine;

public class PartNodeUI : BaseNode
{
    [HideInInspector] public SkillNodeUI skillNode;

    [HideInInspector] public int index;
    [HideInInspector] public bool isSkillConnected;
    private bool _isDisable;
    
    [HideInInspector] public bool isSpecialMode;
    public Action<bool> SpecialModeChangedAction;

    public event Action ReturnInventoryEvent;
    public event Action EquipNodeAction;
    public event Action UnEquipNodeAction;
    public event Action InitNodeAction;
    
    public NodeEquipData CurrentEquipData { get; protected set; }
    public NodeData CurrentNodeData { get; protected set; }
    public bool isEquipDataEmpty => CurrentEquipData == null;
    public bool isPartEmpty => CurrentEquipData?.partInventoryItem?.data == null;

    private Dictionary<Type, IPartNodeUIComponent> _partNodeUIComponents = new ();

    private PartNodeUIChainCheck _uiChainCheck;
    
    protected override void Awake()
    {
        base.Awake();
        
        foreach (var partNodeUIComponent in GetComponentsInChildren<IPartNodeUIComponent>())
            _partNodeUIComponents.Add(partNodeUIComponent.GetType(), partNodeUIComponent);
        foreach (var partNodeUIComponent in _partNodeUIComponents.Values)
            partNodeUIComponent.Initialize(this);
        
        _uiChainCheck = GetCompo<PartNodeUIChainCheck>();
    }

    public void Init(SkillNodeUI skillNodeUI, NodeData nodeData)
    {
        skillNode = skillNodeUI;
        currentSkill = skillNode.currentSkill;
        
        CurrentNodeData = nodeData;
        index = CurrentNodeData.index;

        gameObject.SetActive(true);

        InitializeNodeData();
        UpdateNode(CurrentEquipData);
    }
    
    private void InitializeNodeData()
    {
        if (!skillNode.currentSkillItem.equipNodeData.ContainsKey(index))
        {
            skillNode.currentSkillItem.equipNodeData[index] = new NodeEquipData
            {
                partInventoryItem = null,
                chainList = new List<PartInventoryItem>()
            };
        }
        CurrentEquipData = skillNode.currentSkillItem.equipNodeData[index];
        
        ResetFlags();
    }

    private void ResetFlags()
    {
        _isDisable = false;
    }

    public virtual void UpdateNode(NodeEquipData nodeEquipData)
    {
        skillNode.currentSkillItem.equipNodeData[index] = nodeEquipData;
        CurrentEquipData = nodeEquipData;
        PartEmptyUICheck();
    }
    
    public void NodePartChange(PartInventoryItem changeItem)
    {
        if (CurrentEquipData == null)
        {
            NodeEquipData nodeEquipData = new NodeEquipData
            {
                partInventoryItem = changeItem
            };
            skillNode.currentSkillItem.equipNodeData[index] = nodeEquipData;
            CurrentEquipData = nodeEquipData;
        }
        
        skillNode.currentSkillItem.equipNodeData[index].partInventoryItem = changeItem;
        CurrentEquipData.partInventoryItem = changeItem;

        PartEmptyUICheck();
    }

    private void PartEmptyUICheck()
    {
        if (isPartEmpty)
        {
            ClearSlotUI();
            return;
        }
        
        _uiChainCheck.Init();

        var effect = PoolManager.Instance.Pop(EffectPoolingType.PartNodeEquipEffect) as PartNodeEquipEffect;
        effect.Init(transform, false);
        
        UpdateSlotImage(CurrentEquipData.partInventoryItem.data.icon, Color.white);
    }

    private void ClearSlotUI()
    {
        _uiChainCheck.CleanUp();

        skillNode.currentSkillItem.equipNodeData[index].partInventoryItem = null;
        CurrentEquipData.partInventoryItem = null;

        UpdateSlotImage(null, Color.clear);
    }

    public void UpdateSlotImage(Sprite sprite, Color color, bool disableCheck = false)
    {
        image.sprite = sprite;
        if (disableCheck && _isDisable)
        {
            image.color = Color.gray;
        }
        else
            image.color = color;
    }

    public void UpdateSlotOnlyAlpha(float alpha, bool disableCheck = false)
    {
        if (disableCheck && _isDisable)
        {
            image.color = Color.gray;
            return;
        }
        
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
    public void EquipCurrentSkill()
    {
        if (isPartEmpty)
            return;

        if (_isDisable)
            EnablePart();
        
        EquipNodeAction?.Invoke();
    }

    public void UnEquipCurrentPart()
    {
        if (isPartEmpty || !isSkillConnected)
            return;
        
        UnEquipNodeAction?.Invoke();
    }

    public void InitCurrentPart()
    {
        if (isPartEmpty || !isSkillConnected)
            return;
        
        InitNodeAction?.Invoke();
    }

    public virtual void DisablePart()
    {
        _isDisable = true;
        UpdateSlotImage(image.sprite, Color.gray);
        UnEquipCurrentPart();
    }

    protected virtual void EnablePart()
    {
        _isDisable = false;
        
        Color color = Color.white;
        color.a = isSpecialMode ? 0.3f : 1f;
        UpdateSlotImage(image.sprite, color);
    }

    public void ReturnInventoryItem()
    {
        if (isPartEmpty)
            return;

        if (isSkillConnected)
            UnEquipCurrentPart();

        InventoryManager.Instance.AddInventoryItemWithSo(CurrentEquipData.partInventoryItem.data);
        ReturnInventoryEvent?.Invoke();

        skillNode.currentSkillItem.equipNodeData[index].partInventoryItem = null;
        CurrentEquipData.partInventoryItem = null;

        ResetFlags();

        if (isSkillConnected)
            skillNode.SkillNodeUpdate();

        UpdateSlotImage(null, Color.clear);
    }

    public T GetCompo<T>() where T : IPartNodeUIComponent
    {
        if (_partNodeUIComponents.TryGetValue(typeof(T), out IPartNodeUIComponent component))
            return (T)component;

        return default;
    }
}