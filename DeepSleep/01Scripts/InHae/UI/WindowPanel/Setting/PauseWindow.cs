using DG.Tweening;
using IH.EventSystem.SystemEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using UnityEngine.SceneManagement;
using YH.EventSystem;
using YH.Players;

public class PauseWindow : WindowPanel
{
    [SerializeField] private PlayerInputSO _playerInputSO;
    [SerializeField] private UIInputReader _uiInputReader;
    [SerializeField] private GameEventChannelSO _systemChannelSo;
    [SerializeField] private float _offset;
    [SerializeField] private RectTransform _uiPanel;

    public bool isTween;
    
    private Vector3 _settingOriginPosition;
    
    private void Awake()
    {
        _settingOriginPosition = _uiPanel.position;
    }

    public override void OpenWindow()
    {
        var skillPanelLockEvt = UIPanelEvent.SkillPanelLockEvent;
        skillPanelLockEvt.isLocked = true;
        _uiEventChannel.RaiseEvent(skillPanelLockEvt);
        
        isTween = true;
        _uiPanel.DOLocalMoveY(_settingOriginPosition.y, 0.2f).SetUpdate(true)
            .OnComplete(()=>isTween = false);
    }

    public override void CloseWindow()
    {
        var skillPanelLockEvt = UIPanelEvent.SkillPanelLockEvent;
        skillPanelLockEvt.isLocked = false;
        _uiEventChannel.RaiseEvent(skillPanelLockEvt);
        
        isTween = true;
        _uiPanel.DOLocalMoveY(_settingOriginPosition.y + _offset, 0.2f).SetUpdate(true)
            .OnComplete(()=>isTween = false);
    }
    
    public void GoTitle()
    {
        _systemChannelSo.AddListener<FadeComplete>(HandleFadeComplete);
        
        var evt = SystemEvents.FadeScreenEvent;
        evt.fadeDuration = 0.5f;
        evt.isCircle = true;
        evt.isFadeIn = false;

        _systemChannelSo.RaiseEvent(evt);
    }
    
    private void HandleFadeComplete(FadeComplete evt)
    {
        Time.timeScale = 1;
        _playerInputSO.GetActionMap().Enable();
        _uiInputReader.Controls.Enable();
        _uiInputReader.ClearSubscription();

        _systemChannelSo.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene("TitleScene");
    }
}
