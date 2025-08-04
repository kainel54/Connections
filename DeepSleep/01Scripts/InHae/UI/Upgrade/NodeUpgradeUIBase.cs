using IH.EventSystem.SoundEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using IH.Manager;
using UnityEngine;
using YH.EventSystem;

public abstract class NodeUpgradeUIBase : WindowPanel
{
    [SerializeField] protected Camera _skillUICamera;
    [SerializeField] protected RectTransform _parent;
    
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _uiOpenSound;
    
    private CanvasGroup _canvasGroup;
    private Camera _mainCamera;
    
    protected SkillStash _stash;
    protected SkillInventory _inventory;

    protected virtual void Awake()
    {
        _mainCamera = Camera.main;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    protected virtual void Start()
    {
        _inventory = InventoryManager.Instance.GetInventory(ItemType.Skill) as SkillInventory;
    }

    public override void HandleOpenUI()
    {
        base.HandleOpenUI();
        
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
