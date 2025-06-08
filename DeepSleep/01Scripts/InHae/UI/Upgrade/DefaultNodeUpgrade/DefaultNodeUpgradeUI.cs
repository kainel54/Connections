using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.EventSystem.SoundEvent;
using IH.Manager;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using YH.EventSystem;

public class DefaultNodeUpgradeUI : WindowPanel
{
    [SerializeField] private Camera _skillUICamera;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannel;
    
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _uiOpenSound;
    
    private CanvasGroup _canvasGroup;
    private Camera _mainCamera;
    
    private SkillStash _stash;
    private SkillInventory _inventory;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;

    }

    private void Start()
    {
        _inventory = InventoryManager.Instance.GetInventory(InventoryType.Skill) as SkillInventory;
    }

    public void HandleOpenUI()
    {
        var panelToggleEvent = UIPanelEvent.WindowPanelToggleEvent;
        panelToggleEvent.currentWindow = this;
        _uiEventChannel.RaiseEvent(panelToggleEvent);

        _stash = new SkillStash(_parent, _inventory.GetStash());
        _stash.UpdateSlotUI();
    }
    
    public override void OpenWindow()
    {
        PlaySound();
        
        _mainCamera.gameObject.SetActive(false);
        _skillUICamera.gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;

        var upgradeSkillInitEvent = DefaultNodeUpgradeEvents.UpgradeSkillInitEvent;
        _defaultNodeEventChannel.RaiseEvent(upgradeSkillInitEvent);

        var skillPanelLockEvt = UIPanelEvent.SkillPanelLockEvent;
        skillPanelLockEvt.isLocked = true;
        _uiEventChannel.RaiseEvent(skillPanelLockEvt);
    }

    public override void CloseWindow()
    {
        PlaySound();
        
        _mainCamera.gameObject.SetActive(true);
        _skillUICamera.gameObject.SetActive(false);
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        
        _inventory.SetStash(_stash);
        
        var skillPanelLockEvt = UIPanelEvent.SkillPanelLockEvent;
        skillPanelLockEvt.isLocked = false;
        _uiEventChannel.RaiseEvent(skillPanelLockEvt);
    }
    
    private void PlaySound()
    {
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _uiOpenSound;
        soundPlayEvt.position = transform.position;
        _soundEventChannel.RaiseEvent(soundPlayEvt);
    }
}
