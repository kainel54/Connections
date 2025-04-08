using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _panelParent;
    [SerializeField] private bool _isTweening = false;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _prevButton;

    private int i = 0;
    private float target;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private int _maxPage;

    private void Start()
    {
        target = _panelParent.sizeDelta.x;
        _maxPage = _panelParent.childCount - 1;

        _nextButton.onClick.AddListener(NextPageHandler);
        _prevButton.onClick.AddListener(PrevPageHandler);
    }

    public void NextPageHandler()
    {
        if (_isTweening || i >= _maxPage) return;

        i++;
        MoveTarget(i);
    }

    private void PrevPageHandler()
    {
        if (_isTweening || i <= 0) return;

        i--;
        MoveTarget(i);
    }

    public void MoveTarget(int page)
    {
        _isTweening = true;
        i = page;
        _panelParent.GetComponent<RectTransform>().DOAnchorPosX(target * -page, duration)
            .OnComplete(() => _isTweening = false);
    }
}