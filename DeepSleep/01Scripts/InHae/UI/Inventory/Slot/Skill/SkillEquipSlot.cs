using System;
using System.Collections.Generic;
using IH.Manager;
using IH.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;

public class SkillEquipSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private PlayerInputSO _playerInputSo;
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private GameEventChannelSO _nodeEventChannelSO;
    
    [SerializeField] private int _skillIdx;
    [SerializeField] private WindowPanel _skillInfoUI;
    
    private SkillItemSO _currentSkillData;
    public SkillInventoryItem currentSkillItem;
    private Skill _currentSkill;
    public Skill CurrentSkill => _currentSkill;

    private Image _skillImage;
    private Sprite _defaultSprite;
    private Button _selectBtn;
    
    private Dictionary<string, Type> _skillTypes = new Dictionary<string, Type>();
    
    private RectTransform _dragTarget;
    public bool isEmpty => currentSkillItem == null || currentSkillItem.data == null;  
    private bool _isDragging;
    
    private void Start()
    {
        AddPlayerInputSkill();
        _skillImage = GetComponent<Image>();
        _defaultSprite = _skillImage.sprite;
        _selectBtn = GetComponent<Button>();
        _selectBtn.onClick.AddListener(HandleOpenNodeUI);
    }


    private void OnDestroy()
    {
        _selectBtn.onClick.RemoveListener(HandleOpenNodeUI);
        RemovePlayerInputSkill();
    }
    
    private void AddPlayerInputSkill()
    {
        switch (_skillIdx)
        {
            case 0:
                _playerInputSo.QSkillEvent += HandleSkillInput;
                break;
            case 1:
                _playerInputSo.ESkillEvent += HandleSkillInput;
                break;
            case 2:
                _playerInputSo.FSkillEvent += HandleSkillInput;
                break;
            case 3:
                _playerInputSo.ShiftSkillEvent += HandleSkillInput;
                break;
        }
    }

    private void RemovePlayerInputSkill()
    {
        switch (_skillIdx)
        {
            case 0:
                _playerInputSo.QSkillEvent -= HandleSkillInput;
                break;
            case 1:
                _playerInputSo.ESkillEvent -= HandleSkillInput;
                break;
            case 2:
                _playerInputSo.FSkillEvent -= HandleSkillInput;
                break;
            case 3:
                _playerInputSo.ShiftSkillEvent -= HandleSkillInput;
                break;
        }
    }

    private void HandleSkillInput()
    {
        if(_currentSkill == null)
            return;
        
        _currentSkill.PressSkill();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(_currentSkill != null && _currentSkill.isCoolTime)
            return;
        
        DropItemSlotCase(eventData);
        DropEquipSlotCase(eventData);
    }

    private void DropItemSlotCase(PointerEventData eventData)
    {
        // 드래그한 스킬
        GameObject gameObject = eventData.pointerDrag;
        ItemSlotUI slot = gameObject.GetComponent<ItemSlotUI>();

        if(slot == null || slot.isEmpty)
            return;

        if (!isEmpty)
            InventoryManager.Instance.AddInventoryItem(InventoryType.Skill, currentSkillItem);

        UpdateSlot(slot.item as SkillInventoryItem);
        InventoryManager.Instance.RemoveInventoryItem(InventoryType.Skill, slot.item);
    }
    
    private void DropEquipSlotCase(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerDrag;
        SkillEquipSlot slot = gameObject.GetComponent<SkillEquipSlot>();

        if(slot == null || slot.isEmpty)
            return;
        if(slot.CurrentSkill != null && slot.CurrentSkill.isCoolTime)
            return;
        
        SkillInventoryItem item = currentSkillItem;
        UpdateSlot(slot.currentSkillItem);
        slot.UpdateSlot(item);
    }

    private void UpdateSlot(SkillInventoryItem skillItem)
    {
        currentSkillItem = skillItem;
        if (!isEmpty)
        {
            _currentSkillData = currentSkillItem.data as SkillItemSO;
            _skillImage.sprite = _currentSkillData.icon;
            
            if (!_skillTypes.ContainsKey(_currentSkillData.reflectionName))
                _skillTypes.Add(_currentSkillData.reflectionName, Type.GetType(_currentSkillData.reflectionName));
            
            if(_currentSkill != null)
                Destroy(_currentSkill.gameObject);
            
            _currentSkill = Instantiate(SkillManager.Instance.GetSkill(_skillTypes[_currentSkillData.reflectionName]),
                transform);
        }
        else
        {
            CleanUp();
        }
        
        SkillHudUpdate();
    }

    public void Init()
    {
        InventoryManager.Instance.AddInventoryItem(InventoryType.Skill, currentSkillItem);
        CleanUp();
        SkillHudUpdate();
    }

    private void CleanUp()
    {
        _skillImage.sprite = _defaultSprite;
        currentSkillItem = null;
        if(_currentSkill != null)
            Destroy(_currentSkill.gameObject);
        _currentSkillData = null;
    }

    private void HandleOpenNodeUI()
    {
        if(isEmpty || _isDragging)
            return;
        
        var evt = UIEvents.WindowPanelOpenEvent;
        evt.currentWindow = _skillInfoUI;
        _uiEventChannelSO.RaiseEvent(evt);

        var evt2 = NodeEvents.InitNodeSkillEvent;
        evt2.skillInventoryItem = currentSkillItem;
        evt2.skill = _currentSkill;
        _nodeEventChannelSO.RaiseEvent(evt2);
    }
    
    private void SkillHudUpdate()
    {
        var evt = UIEvents.SkillHudEvent;
        evt.SkillItemData = _currentSkillData;
        evt.skill = _currentSkill;
        evt.skillIdx = _skillIdx;

        _uiEventChannelSO.RaiseEvent(evt);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(isEmpty)
            return;
        
        _isDragging = true;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.SkillAndPart);
        dragItem.StartDrag(currentSkillItem);
        _dragTarget = dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;
        _skillImage.color =Color.clear;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(isEmpty)
            return;
        
        _dragTarget.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        _isDragging = false;

        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.SkillAndPart);
        dragItem.EndDrag();
        _skillImage.color = Color.white;
    }
}
