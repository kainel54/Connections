using System;
using System.Collections.Generic;
using IH.EventSystem;
using IH.Manager;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class PartNodeChainCheck : MonoBehaviour
{
    [SerializeField] private ChainAbleListSO _chainAbleListSO;
    [SerializeField] private GameEventChannelSO _nodeEventChannel;
    [SerializeField] private Image _cantChainImage;
    [SerializeField] private Image _canChainImage;
    [SerializeField] private float _yOffset;
    
    [HideInInspector] public bool isChainAble;
    
    private bool _isChainListOpen;
    private PartNodeUI _nodeUI;
    private Button _button;
    private ChainPopUpPanel _chainPopUpPanel;
    
    public PartNode chainPartNode;
    private ChainDataList _currentChainData;
    
    private List<PartInventoryItem> _cachedChainList;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _nodeUI = GetComponent<PartNodeUI>();
        
        _nodeUI.ReturnInventoryEvent += ChainAllReturnInventory;
        _nodeEventChannel.AddListener<ChainPartSelectEvent>(HandlePartSelect);
        _nodeEventChannel.AddListener<ChainPartSelectCompleteEvent>(HandlePartComplete);
        _button.onClick.AddListener(ChainListPopUp);
        
        _chainPopUpPanel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Chain) as ChainPopUpPanel;
    }

    private void OnDestroy()
    {
        _nodeUI.ReturnInventoryEvent -= ChainAllReturnInventory;
        _nodeEventChannel.RemoveListener<ChainPartSelectEvent>(HandlePartSelect);
        _nodeEventChannel.RemoveListener<ChainPartSelectCompleteEvent>(HandlePartComplete);
    }

    public void Init()
    {
        _currentChainData = null;
        _nodeUI.isChained = false;
        CacheChainList();

        if(FindCanChainNode())
            ChainApplyNode();
    }
    
    private void HandlePartSelect(ChainPartSelectEvent evt)
    {
        if (_nodeUI.isEmpty)
            return;
        
        _currentChainData = new ChainDataList();
        _currentChainData.parts.Add(evt.partItemSO);
        _currentChainData.parts.Add(_nodeUI.CurrentData.partInventoryItem.data as PartItemSO);
        
        foreach (var item in _nodeUI.CurrentData.chainList)
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
        if(_nodeUI.CurrentData == null)
            return;
        
        if (_isChainListOpen)
            _chainPopUpPanel.EndPopUp();
        else
        {
            Vector3 pos = Input.mousePosition;
            pos.y -= _yOffset;
            _chainPopUpPanel.transform.position = pos;
            _chainPopUpPanel.ChainListPopUp(_nodeUI.CurrentData.chainList, this);
        }

        _isChainListOpen = !_isChainListOpen;
    }
    
    public void AddChainItem(PartInventoryItem item)
    {
        _nodeUI.CurrentData.chainList.Add(item);
        CacheChainList();

        if (!FindCanChainNode())
            return;
    
        ChainApplyNode();
        _nodeUI.skillNode.StartNodeCheck();
    }

    public void RemoveChainPart(PartInventoryItem item)
    {
        _nodeUI.CurrentData.chainList.Remove(item);
        CacheChainList();

        if(FindCanChainNode())
            ChainApplyNode();
        else
            CantChain();
    
        _nodeUI.skillNode.StartNodeCheck();
    }
    private bool FindCanChainNode()
    {
        if (_cachedChainList == null) CacheChainList();

        _currentChainData = new ChainDataList();
        _currentChainData.parts.Add(_nodeUI.CurrentData.partInventoryItem.data as PartItemSO);
        foreach (var item in _cachedChainList)
            _currentChainData.parts.Add(item.data as PartItemSO);

        return _chainAbleListSO.List.Contains(_currentChainData);
    }
    
    private void ChainApplyNode()
    {
        _nodeUI.isChained = true;
        chainPartNode = Activator.CreateInstance(_chainAbleListSO.nodeDictionary[_currentChainData]) as PartNode;
        chainPartNode.partType = _nodeUI.CurrentData.partInventoryItem.partNode.partType;
    }

    private void CantChain()
    {
        ChainAllReturnInventory();
        _nodeUI.isChained = false;
    }
    
    private void ChainAllReturnInventory()
    {
        foreach (var item in _nodeUI.CurrentData.chainList)
            InventoryManager.Instance.AddInventoryItemWithSo(item.data);

        CleanUp();
    }

    public void CleanUp()
    {
        _nodeUI.isChained = false;
        _currentChainData = null;
        chainPartNode = null;
        _nodeUI.CurrentData?.chainList.Clear();
    }

    private void CacheChainList()
    {
        _cachedChainList = new List<PartInventoryItem>(_nodeUI.CurrentData.chainList);
    }
    
    private void PanelUpdate() => _chainPopUpPanel.UpdatePopUp(_nodeUI.CurrentData.chainList, this);
}
