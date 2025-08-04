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
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;
using TMPro;
using UnityEngine.InputSystem;

public class SkillEquipSlot : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManager;
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private GameEventChannelSO _skillNodeEventChannelSO;
    [SerializeField] private GameEventChannelSO _missionEventChannelSO;
    [SerializeField] private GameEventChannelSO _levelEventChannelSO;
    
    public int skillIdx;
    [SerializeField] private WindowPanel _nodeViewUI;
    
    private Dictionary<string, Type> _skillTypes = new ();
    
    public SkillInventoryItem currentSkillItem;
    private SkillItemSO _currentSkillData;
    private Skill _currentSkill;
    public Skill CurrentSkill => _currentSkill;

    private Image _skillImage;
    private Sprite _defaultSprite;
    private Button _selectBtn;
    private TextMeshProUGUI _keyboard;
    
    public bool IsEmpty => currentSkillItem == null || currentSkillItem.data == null;  
    
    private bool _isCombat;
    public bool IsCombat => _isCombat;
    private bool _isOnlyNormalAttackEventing;
    
    private SkillEquipSlotPointerAction _slotPointerAction;

    private void Start()
    {
        _slotPointerAction = GetComponent<SkillEquipSlotPointerAction>();

        _playerManager.SetUpPlayerEvent += HandleSetPlayer;
        
        _skillImage = GetComponent<Image>();
        _defaultSprite = _skillImage.sprite;
        
        _selectBtn = GetComponent<Button>();
        _selectBtn.onClick.AddListener(HandleOpenNodeUI);
        _keyboard = GetComponentInChildren<TextMeshProUGUI>();

        _missionEventChannelSO.AddListener<OnlyNormalAttackMissionStartEvent>(HandleCheckInputSkill);
        _levelEventChannelSO.AddListener<InCombatCheckEvent>(HandleInCombatCheck);
    }

    private void HandleSetPlayer()
    {
        _playerManager.Player.PlayerInput.SkillActions[skillIdx] += HandleSkillInput;
        _keyboard.text = _playerManager.Player.PlayerInput.GetSkillKeyName(skillIdx);
    }

    private void OnDestroy()
    {
        _playerManager.SetUpPlayerEvent -= HandleSetPlayer;

        _playerManager.Player.PlayerInput.SkillActions[skillIdx] -= HandleSkillInput;
        
        _selectBtn.onClick.RemoveListener(HandleOpenNodeUI);
        
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
        
        _currentSkill.SkillAnimation.CheckPlaySkillAnimation();
    }

    public void UpdateSlot(SkillInventoryItem skillItem)
    {
        currentSkillItem = skillItem;
        if (!IsEmpty)
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
            
            var partInfoInitEvt = SkillNodeEvents.EquipPartInfoInitEvent;
            _skillNodeEventChannelSO.RaiseEvent(partInfoInitEvt);
            
            var nodeInitEvt = SkillNodeEvents.SkillNodeInitEvent;
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

    public void SetSkillImageColor(Color color) => _skillImage.color = color;
    
    public void Init()
    {
        var skillUnEquipCheckEvent = SkillNodeEvents.SkillUnEquipCheckEvent;
        skillUnEquipCheckEvent.skill = _currentSkill;
        _skillNodeEventChannelSO.RaiseEvent(skillUnEquipCheckEvent);
        
        InventoryManager.Instance.AddInventoryItem(ItemType.Skill, currentSkillItem);
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
        if(IsEmpty || _slotPointerAction.IsDragging || IsCombatCheck())
            return;
        
        var nodeViewOpenEvt = UIPanelEvent.WindowPanelOpenEvent;
        nodeViewOpenEvt.currentWindow = _nodeViewUI;
        _uiEventChannelSO.RaiseEvent(nodeViewOpenEvt);

        var nodeInitEvt = SkillNodeEvents.SkillNodeInitEvent;
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
    public bool IsCombatCheck()
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
