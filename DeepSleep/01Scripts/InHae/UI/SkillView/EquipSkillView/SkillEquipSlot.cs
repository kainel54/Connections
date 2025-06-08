using System;
using System.Collections.Generic;
using IH.EventSystem.LevelEvent;
using IH.EventSystem.MissionEvent;
using IH.Manager;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.UIEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using ObjectPooling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;

public class SkillEquipSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, 
    IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private PlayerInputSO _playerInputSo;
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private GameEventChannelSO _skillNodeEventChannelSO;
    [SerializeField] private GameEventChannelSO _missionEventChannelSO;
    [SerializeField] private GameEventChannelSO _levelEventChannelSO;
    
    public int skillIdx;
    [SerializeField] private WindowPanel _nodeViewUI;
    
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

    private bool _isOnlyNormalAttackEventing;
    
    private bool _isCombat;
    public bool isCombat => _isCombat;

    private void Start()
    {
        _playerInputSo.SkillActions[skillIdx] += HandleSkillInput;
        
        _skillImage = GetComponent<Image>();
        _defaultSprite = _skillImage.sprite;
        
        _selectBtn = GetComponent<Button>();
        _selectBtn.onClick.AddListener(HandleOpenNodeUI);
        
        _missionEventChannelSO.AddListener<OnlyNormalAttackMissionStartEvent>(HandleCheckInputSkill);
        _levelEventChannelSO.AddListener<InCombatCheckEvent>(HandleInCombatCheck);
    }
    
    private void OnDestroy()
    {
        _selectBtn.onClick.RemoveListener(HandleOpenNodeUI);
        
        _playerInputSo.SkillActions[skillIdx] -= HandleSkillInput;
        
        _missionEventChannelSO.RemoveListener<OnlyNormalAttackMissionStartEvent>(HandleCheckInputSkill);
        _levelEventChannelSO.RemoveListener<InCombatCheckEvent>(HandleInCombatCheck);
    }
    
    private void HandleInCombatCheck(InCombatCheckEvent evt)
    {
        _isCombat = evt.isCombat;
    }
    
    private void HandleCheckInputSkill(OnlyNormalAttackMissionStartEvent evt)
    {
        _isOnlyNormalAttackEventing = evt.isStart;
        if(_currentSkill == null)
            return;
        
        if(evt.isStart)
            _currentSkill.PressAction += HandleSkillInputMissionCheck;
        else
            _currentSkill.PressAction -= HandleSkillInputMissionCheck;
    }

    private void HandleSkillInputMissionCheck()
    {
        _currentSkill.PressAction -= HandleSkillInputMissionCheck;

        var onlyAttackMissionFail = MissionEvents.OnlyNormalAttackMissionFailCheckEvent;
        _missionEventChannelSO.RaiseEvent(onlyAttackMissionFail);
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
        if(IsCombatCheck())
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

        if (slot == null || slot.isEmpty)
            return;
        if (slot.CurrentSkill != null && slot.CurrentSkill.isCoolTime)
            return;
        
        SkillInventoryItem tempData = slot.currentSkillItem;
        slot.UpdateSlot(currentSkillItem);
        UpdateSlot(tempData);
    }

    public void UpdateSlot(SkillInventoryItem skillItem)
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
            
            _currentSkill = Instantiate
                (SkillManager.Instance.GetSkill(_skillTypes[_currentSkillData.reflectionName]), transform);

            if (_isOnlyNormalAttackEventing)
                _currentSkill.PressAction += HandleSkillInputMissionCheck;
            
            var nodeInitEvt = SkillNodeEvents.InitNodeSkillEvent;
            nodeInitEvt.skillInventoryItem = currentSkillItem;
            nodeInitEvt.skill = _currentSkill;
            _skillNodeEventChannelSO.RaiseEvent(nodeInitEvt);
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
        if(isEmpty || _isDragging || IsCombatCheck())
            return;
        
        var nodeViewOpenEvt = UIPanelEvent.WindowPanelOpenEvent;
        nodeViewOpenEvt.currentWindow = _nodeViewUI;
        _uiEventChannelSO.RaiseEvent(nodeViewOpenEvt);

        var nodeInitEvt = SkillNodeEvents.InitNodeSkillEvent;
        nodeInitEvt.skillInventoryItem = currentSkillItem;
        nodeInitEvt.skill = _currentSkill;
        _skillNodeEventChannelSO.RaiseEvent(nodeInitEvt);
    }
    
    private void SkillHudUpdate()
    {
        var evt = UIEvents.SkillHudEvent;
        evt.SkillItemData = _currentSkillData;
        evt.skill = _currentSkill;
        evt.skillIdx = skillIdx;

        _uiEventChannelSO.RaiseEvent(evt);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(isEmpty || IsCombatCheck())
            return;
        
        _isDragging = true;
        
        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        dragItem.StartDrag(currentSkillItem);
        _dragTarget = dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;
        _skillImage.color =Color.clear;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        if(isEmpty || _isCombat)
            return;
        
        _dragTarget.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left || _isCombat)
            return;
        
        _isDragging = false;

        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.InventorySlotItem);
        dragItem.EndDrag();
        _skillImage.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Right || IsCombatCheck())
            return;
        
        Init();
    }

    private bool IsCombatCheck()
    {
        if (!_isCombat)
            return _isCombat;
        
        UIPopUpText uiPopUp = PoolManager.Instance.Pop(UIPoolingType.UIPopUpText) as UIPopUpText;
        uiPopUp.transform.SetParent(transform as RectTransform);
        uiPopUp.TextInit("전투 중에는\n변경이 불가능합니다!", 30f, Color.red, transform.position);
        uiPopUp.UpAndFadeText(50f, 1f);
        
        return _isCombat;
    }
}
