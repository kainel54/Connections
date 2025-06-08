using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.SoundEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class SkillUIWindow : WindowPanel
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _uiOpenSound;
    
    [SerializeField] private UIInputReader _uiInputReader;
    [SerializeField] private Camera _skillUICamera;
    [SerializeField] private Scrollbar _skillItemScrollbar;
    
    private CanvasGroup _canvasGroup;
    private Camera _mainCamera;
    
    private NodeUIWindow _nodeUIWindow;
    
    private bool _isLocked;
    
    private void Awake()
    {
        _uiEventChannel.AddListener<SkillPanelLockEvent>(HandleSkillPanelLockEvent);
        _nodeUIWindow = GetComponentInChildren<NodeUIWindow>();
        
        _mainCamera = Camera.main;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        
        _uiInputReader.TapEvent += HandleOpenSkillUI;
    }

    private void OnDestroy()
    {
        _uiEventChannel.RemoveListener<SkillPanelLockEvent>(HandleSkillPanelLockEvent);
        _uiInputReader.TapEvent -= HandleOpenSkillUI;
    }

    private void HandleSkillPanelLockEvent(SkillPanelLockEvent evt) => _isLocked = evt.isLocked;

    private void HandleOpenSkillUI()
    {
        if(_isLocked)
            return;
        
        _skillItemScrollbar.value = 1;
        
        var evt = UIPanelEvent.WindowPanelToggleEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);
    }

    public override void OpenWindow()
    {
        PlaySound();
        Time.timeScale = 0f;

        var equipPartInfoInitEvt = SkillNodeEvents.EquipPartInfoInitEvent;
        _skillNodeEventChannel.RaiseEvent(equipPartInfoInitEvt);
        
        _mainCamera.gameObject.SetActive(false);
        _skillUICamera.gameObject.SetActive(true);
        
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public override void CloseWindow()
    {
        PlaySound();
        Time.timeScale = 1f;
        
        var evt =  UIPanelEvent.WindowPanelCloseEvent;
        evt.currentWindow = _nodeUIWindow;
        _uiEventChannel.RaiseEvent(evt);
        
        _mainCamera.gameObject.SetActive(true);
        _skillUICamera.gameObject.SetActive(false);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    private void PlaySound()
    {
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _uiOpenSound;
        soundPlayEvt.position = transform.position;
        _soundEventChannel.RaiseEvent(soundPlayEvt);
    }
}
