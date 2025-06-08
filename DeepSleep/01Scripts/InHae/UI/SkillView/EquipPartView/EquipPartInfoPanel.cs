using System.Collections.Generic;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class EquipPartInfoPanel : MonoBehaviour
{
    [SerializeField] private UIInputReader _uiInputReader;
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private EquipPartInfo _equipPartInfo;
    [SerializeField] private RectTransform _equipPartParent;
    
    [SerializeField] private Image _skillImage;
    private TextMeshProUGUI _skillTitle;
    private TextMeshProUGUI _skillDescription;
    
    private Scrollbar _scrollbar;
    private SkillInventoryItem _skillInventoryItem;

    private bool _isFirstInfoInit;

    private void Awake()
    {
        _scrollbar = GetComponentInChildren<Scrollbar>();

        _skillTitle = transform.Find("TopGroup/SkillName").GetComponent<TextMeshProUGUI>();
        _skillDescription = transform.Find("TopGroup/Description").GetComponent<TextMeshProUGUI>();
        
        _skillNodeEventChannel.AddListener<EquipPartInfoEvent>(HandleEquipPartInfoEvent);
        _skillNodeEventChannel.AddListener<EquipPartInfoInitEvent>(HandlePartInfoInitEvent);
        _skillNodeEventChannel.AddListener<EquipSkillSelectEvent>(HandleEquipSkillSelectEvent);

        _uiInputReader.XKeyEvent += HandleAllPartUnEquipEvent;
    }

    private void OnDestroy()
    {
        _skillNodeEventChannel.RemoveListener<EquipPartInfoEvent>(HandleEquipPartInfoEvent);
        _skillNodeEventChannel.RemoveListener<EquipPartInfoInitEvent>(HandlePartInfoInitEvent);
        _skillNodeEventChannel.RemoveListener<EquipSkillSelectEvent>(HandleEquipSkillSelectEvent);
        
        _uiInputReader.XKeyEvent -= HandleAllPartUnEquipEvent;
    }

    private void HandleEquipSkillSelectEvent(EquipSkillSelectEvent evt) => Init();
    private void HandlePartInfoInitEvent(EquipPartInfoInitEvent evt) => Init();
    
    private void HandleEquipPartInfoEvent(EquipPartInfoEvent evt)
    {
        if (!_isFirstInfoInit)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
            
            _isFirstInfoInit = true;
        }
        
        for (int i = 0; i < _equipPartParent.childCount; i++)
            Destroy(_equipPartParent.GetChild(i).gameObject);
        
        _scrollbar.value = 1;
        _skillInventoryItem = evt.skillInventoryItem;
        SetImageAndText();

        foreach (var equipPart in _skillInventoryItem.equipNodeData.Values)
        {
            if(equipPart?.partInventoryItem == null || equipPart.partInventoryItem.data == null)
                continue;

            EquipPartInfo equipPartInfo = Instantiate(_equipPartInfo, _equipPartParent);
            equipPartInfo.UpdateSlot(equipPart.partInventoryItem);
        }
    }

    private void SetImageAndText()
    {
        _skillImage.sprite = _skillInventoryItem.data.icon;
        _skillTitle.text = _skillInventoryItem.data.itemName;
        _skillDescription.text = _skillInventoryItem.data.itemDescription;
    }
    
    private void Init()
    {
        _skillInventoryItem = null;
        _isFirstInfoInit = false;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
    
    // 나중에 특별 노드가 추가되면 특별 노드도 인벤에 돌아오도록 해줘야 함
    private void HandleAllPartUnEquipEvent()
    {
        if(_skillInventoryItem == null || _skillInventoryItem.data == null)
            return;

        foreach (var part in _skillInventoryItem.equipNodeData)
        {
            NodeEquipData currentEquipData = _skillInventoryItem.equipNodeData[part.Key];
            if(currentEquipData == null || currentEquipData.partInventoryItem == null)
                continue;
            
            foreach (var chainItem in currentEquipData.chainList)
                InventoryManager.Instance.AddInventoryItemWithSo(chainItem.data);
            InventoryManager.Instance.AddInventoryItemWithSo(currentEquipData.partInventoryItem.data);
        }
        _skillInventoryItem.equipNodeData = new Dictionary<int, NodeEquipData>();
        
        for (int i = 0; i < _equipPartParent.childCount; i++)
            Destroy(_equipPartParent.GetChild(i).gameObject);
    }
}
