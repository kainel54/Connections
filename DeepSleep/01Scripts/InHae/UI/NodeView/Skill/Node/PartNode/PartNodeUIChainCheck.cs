using System;
using System.Collections.Generic;
using IH.EventSystem.NodeEvent.NodeChainEvent;
using IH.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class PartNodeUIChainCheck : MonoBehaviour, IPartNodeUIComponent
{
    [SerializeField] private ChainAbleListSO _chainAbleListSO;
    [SerializeField] private GameEventChannelSO _nodeChainEventChannel;
    
    [SerializeField] private Image _cantChainImage;
    [SerializeField] private Image _canChainImage;
    [SerializeField] private float _yOffset;
    
    [HideInInspector] public bool isChainAble;
    [HideInInspector] public bool isChainMode;
    [HideInInspector] public bool isChained;

    private bool _isChainListOpen;
    private PartNodeUI _partNodeUI;
    private Button _chainListButton;
    private TextMeshProUGUI _chainButtonText;
    private ChainPopUpPanel _chainPopUpPanel;
    
    public PartNode chainPartNode;
    private ChainDataList _currentChainData;
    
    private List<PartInventoryItem> _cachedChainList;

    private void Awake()
    {
        _chainListButton = GetComponentInChildren<Button>();
        _chainButtonText = _chainListButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        
        _chainListButton.onClick.AddListener(ChainListPopUp);
        _chainPopUpPanel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Chain) as ChainPopUpPanel;
    }
    
    public void Initialize(PartNodeUI partNodeUI)
    {
        _partNodeUI = partNodeUI;
        _partNodeUI.ReturnInventoryEvent += ChainAllReturnInventory;
        
        _nodeChainEventChannel.AddListener<ChainModeChangeEvent>(HandleChangeMode);
        _nodeChainEventChannel.AddListener<ChainPartSelectEvent>(HandlePartSelect);
        _nodeChainEventChannel.AddListener<ChainPartSelectCompleteEvent>(HandlePartComplete);
    }

    private void OnDestroy()
    {
        _partNodeUI.ReturnInventoryEvent -= ChainAllReturnInventory;
        
        _nodeChainEventChannel.RemoveListener<ChainModeChangeEvent>(HandleChangeMode);
        _nodeChainEventChannel.RemoveListener<ChainPartSelectEvent>(HandlePartSelect);
        _nodeChainEventChannel.RemoveListener<ChainPartSelectCompleteEvent>(HandlePartComplete);
    }

    public void Init()
    {
        _currentChainData = null;
        isChained = false;
        CacheChainList();
        ChainListButtonText();
        
        if(FindCanChainNode())
            ChainApplyNode();
    }
    
    private void HandleChangeMode(ChainModeChangeEvent evt)
    {
        isChainMode = evt.isActive;
    }
    
    private void HandlePartSelect(ChainPartSelectEvent evt)
    {
        if (_partNodeUI.isPartEmpty)
            return;
        
        _currentChainData = new ChainDataList();
        _currentChainData.parts.Add(evt.partItemSO);
        _currentChainData.parts.Add(_partNodeUI.CurrentEquipData.partInventoryItem.data as PartItemSO);
        
        foreach (var item in _partNodeUI.CurrentEquipData.chainList)
            _currentChainData.parts.Add(item.data as PartItemSO);
        
        if (_chainAbleListSO.List.Contains(_currentChainData))
        {
            isChainAble = true;
            _canChainImage.gameObject.SetActive(true);
        }
        else
        {
            isChainAble = false;
            _cantChainImage.gameObject.SetActive(true);
        }
    }
    
    private void HandlePartComplete(ChainPartSelectCompleteEvent evt)
    {
        isChainAble = false;
        _canChainImage.gameObject.SetActive(false);
        _cantChainImage.gameObject.SetActive(false);
    }
    
    public void FindChainNode()
    {
        ChainApplyNode();
    }

    private void ChainListPopUp()
    {
        if(_partNodeUI.CurrentEquipData?.partInventoryItem == null)
            return;
        _chainPopUpPanel.ChainListPopUp(_partNodeUI.CurrentEquipData.chainList, this);
    }
    
    public void AddChainItem(PartInventoryItem item)
    {
        _partNodeUI.CurrentEquipData.chainList.Add(item);
        CacheChainList();
        ChainListButtonText();
        
        if (!FindCanChainNode())
            return;
        
        _chainPopUpPanel.ChainListPopUp(_partNodeUI.CurrentEquipData.chainList, this);
        ChainApplyNode();
        _partNodeUI.skillNode.SkillNodeUpdate();
    }

    public void RemoveChainPart(PartInventoryItem item)
    {
        _partNodeUI.CurrentEquipData.chainList.Remove(item);
        CacheChainList();
        ChainListButtonText();
        
        if(FindCanChainNode())
            ChainApplyNode();
        else
            CantChain();
        
        _chainPopUpPanel.ChainListPopUp(_partNodeUI.CurrentEquipData.chainList, this);
        _partNodeUI.skillNode.SkillNodeUpdate();
    }
    
    private bool FindCanChainNode()
    {
        if (_cachedChainList == null) CacheChainList();

        _currentChainData = new ChainDataList();
        _currentChainData.parts.Add(_partNodeUI.CurrentEquipData.partInventoryItem.data as PartItemSO);
        foreach (var item in _cachedChainList)
            _currentChainData.parts.Add(item.data as PartItemSO);

        return _chainAbleListSO.List.Contains(_currentChainData);
    }
    
    private void ChainApplyNode()
    {
        isChained = true;
        chainPartNode = Activator.CreateInstance(_chainAbleListSO.nodeDictionary[_currentChainData]) as PartNode;
        chainPartNode.partType = _partNodeUI.CurrentEquipData.partInventoryItem.partNode.partType;
    }

    private void CantChain()
    {
        ChainAllReturnInventory();
        isChained = false;
    }
    
    private void ChainAllReturnInventory()
    {
        foreach (var item in _partNodeUI.CurrentEquipData.chainList)
            InventoryManager.Instance.AddInventoryItemWithSo(item.data);

        _chainPopUpPanel.EndPopUp();
        CleanUp();
    }

    public void CleanUp()
    {
        isChained = false;
        _currentChainData = null;
        chainPartNode = null;
        _chainButtonText.text = "";
        _partNodeUI.CurrentEquipData?.chainList.Clear();
    }

    private void CacheChainList()
    {
        _cachedChainList = new List<PartInventoryItem>(_partNodeUI.CurrentEquipData.chainList);
    }

    private void ChainListButtonText()
    {
        _chainButtonText.text = _cachedChainList.Count > 0 ? "체인 확인" : "";
    }
}