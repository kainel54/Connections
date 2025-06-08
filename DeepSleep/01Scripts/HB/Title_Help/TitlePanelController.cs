using DG.Tweening;
using IH.EventSystem.SystemEvent;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YH.EventSystem;

public class TitlePanelController : MonoBehaviour
{
    private CanvasGroup _helpCanvasGroup;

    [SerializeField] private RectTransform _helpPanel;
    [SerializeField] private Image _background;

    [SerializeField] private Vector2 sizeDelta;
    private bool _isHelpPanelOpened = false;

    [SerializeField] private GameEventChannelSO _systemChannel;

    public UnityEvent closeEvent;

    private void Start()
    {
        _helpCanvasGroup = GetComponent<CanvasGroup>();

        sizeDelta = _helpPanel.sizeDelta;
        
        _helpCanvasGroup.interactable = false;
        _helpCanvasGroup.blocksRaycasts = false;
        _helpPanel.DOSizeDelta(new Vector2(0, sizeDelta.y), 0);
        _background.DOFade(0, 0);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_isHelpPanelOpened)
                CloseHelpPanelHandler();
        }
    }

    public void GameStartHandler()
    {
        _systemChannel.AddListener<FadeComplete>(HandleSceneMove);

        var fadeOutEvent = SystemEvents.FadeScreenEvent;
        fadeOutEvent.isFadeIn = false;
        fadeOutEvent.isCircle = true;
        fadeOutEvent.fadeDuration = 0.2f;

        _systemChannel.RaiseEvent(fadeOutEvent);
    }

    private void HandleSceneMove(FadeComplete evt)
    {
        _systemChannel.RemoveListener<FadeComplete>(HandleSceneMove);
        SceneManager.LoadScene("MergeScene");
    }

    public void OpenHelpPanelHandler()
    {
        _helpPanel.DOSizeDelta(sizeDelta, 0.3f);
        _background.DOFade(0.8f, 0.3f)
            .OnComplete(() =>
            {
                _helpCanvasGroup.interactable = true;
                _helpCanvasGroup.blocksRaycasts = true;
                _isHelpPanelOpened = true;
            });
    }

    public void CloseHelpPanelHandler()
    {
        closeEvent?.Invoke();
        _helpCanvasGroup.interactable = false;
        _helpCanvasGroup.blocksRaycasts = false;
        
        _isHelpPanelOpened = false;
        _helpPanel.DOSizeDelta(new Vector2(0, sizeDelta.y), 0.3f);
        _background.DOFade(0, 0.3f);
    }

    public void QuitGameHandler()
    {
        Application.Quit();
    }
}
