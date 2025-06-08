using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.EventSystem.SoundEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using IH.Manager;
using UnityEngine;
using YH.EventSystem;

public class SpecialNodeUpgradeUI : WindowPanel
{
    [SerializeField] private Camera _skillUICamera;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private GameEventChannelSO _specialNodeUpgradeEventChannel;
    
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
        var evt = UIPanelEvent.WindowPanelToggleEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);

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

        var evt = SpecialNodeUpgradeEvents.UpgradeSkillInitEvent;
        _specialNodeUpgradeEventChannel.RaiseEvent(evt);
        
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
