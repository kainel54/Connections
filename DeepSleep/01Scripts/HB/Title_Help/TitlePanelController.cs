using DG.Tweening;
using IH.EventSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YH.EventSystem;

public class TitlePanelController : MonoBehaviour
{
    private CanvasGroup _helpCanvasGroup;

    [SerializeField] private RectTransform _helpPanel;
    [SerializeField] private Image _background;

    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private Vector2 sizeDelta;

    [SerializeField] private GameEventChannelSO _systemChannel;
    
    public UnityEvent closeEvent;

    private void Start()
    {
        _helpCanvasGroup = transform.GetComponent<CanvasGroup>();

        _gameStartButton.onClick.AddListener(GameStartHandler);
        _helpButton.onClick.AddListener(OpenHelpPanelHandler);
        _closeButton.onClick.AddListener(CloseHelpPanelHandler);
        _quitButton.onClick.AddListener(QuitGameHandler);

        sizeDelta = _helpPanel.sizeDelta;

        _helpCanvasGroup.interactable = false;
        _helpCanvasGroup.blocksRaycasts = false;
        _helpPanel.DOSizeDelta(new Vector2(0, sizeDelta.y), 0);
        _background.DOFade(0, 0);
    }

    private void GameStartHandler()
    {
        // 게임 시작
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

    private void OpenHelpPanelHandler()
    {
        _helpPanel.DOSizeDelta(sizeDelta, 0.3f);
        _background.DOFade(1, 0.3f)
            .OnComplete(() =>
            {
                _helpCanvasGroup.interactable = true;
                _helpCanvasGroup.blocksRaycasts = true;
            });
    }

    private void CloseHelpPanelHandler()
    {
        closeEvent?.Invoke();
        _helpCanvasGroup.interactable = false;
        _helpCanvasGroup.blocksRaycasts = false;
        _helpPanel.DOSizeDelta(new Vector2(0, sizeDelta.y), 0.3f);
        _background.DOFade(0, 0.3f);
    }

    private void QuitGameHandler()
    {
        Application.Quit();
    }
}
