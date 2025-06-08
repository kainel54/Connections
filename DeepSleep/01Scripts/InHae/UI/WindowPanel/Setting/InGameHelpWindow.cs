using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InGameHelpWindow : WindowPanel
{
    private CanvasGroup _helpCanvasGroup;

    [SerializeField] private RectTransform _helpPanel;
    [SerializeField] private Image _background;

    private Vector2 _sizeDelta;
    private bool _isHelpPanelOpened = false;

    private void Start()
    {
        _helpCanvasGroup = GetComponent<CanvasGroup>();

        _sizeDelta = _helpPanel.sizeDelta;
        
        _helpCanvasGroup.alpha = 0;
        _helpCanvasGroup.interactable = false;
        _helpCanvasGroup.blocksRaycasts = false;
        
        _helpPanel.DOSizeDelta(new Vector2(0, _sizeDelta.y), 0);
        _background.DOFade(0, 0);
    }

    public override void OpenWindow()
    {
        _helpCanvasGroup.alpha = 1;
        
        _helpPanel.DOSizeDelta(_sizeDelta, 0.3f).SetUpdate(true);
        _background.DOFade(0.8f, 0.3f).SetUpdate(true)
            .OnComplete(() =>
            {
                _helpCanvasGroup.interactable = true;
                _helpCanvasGroup.blocksRaycasts = true;
                _isHelpPanelOpened = true;
            });
    }

    public override void CloseWindow()
    {
        _helpCanvasGroup.interactable = false;
        _helpCanvasGroup.blocksRaycasts = false;
        
        _isHelpPanelOpened = false;
        _helpPanel.DOSizeDelta(new Vector2(0, _sizeDelta.y), 0.3f).SetUpdate(true);
        _background.DOFade(0, 0.3f).SetUpdate(true).OnComplete(() => _helpCanvasGroup.alpha = 0);
    }
}
