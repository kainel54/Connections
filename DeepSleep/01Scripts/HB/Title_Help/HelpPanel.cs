using DG.Tweening;
using System.Collections.Generic;
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
    private int _maxPage;
    [SerializeField] private float duration = 0.3f;

    [SerializeField] private List<Image> _quickButtonList;
    [SerializeField] private Transform _quickButtonParent;

    private void Start()
    {
        target = _panelParent.sizeDelta.x;
        _maxPage = _panelParent.childCount - 1;

        _quickButtonList = new List<Image>();
        foreach (Image children in _quickButtonParent.GetComponentsInChildren<Image>())
        {
            _quickButtonList.Add(children);
            children.color = Color.gray;
        }

        _nextButton.onClick.AddListener(NextPageHandler);
        _prevButton.onClick.AddListener(PrevPageHandler);
    }

    private void NextPageHandler()
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

        foreach(var currentButton in _quickButtonList)
        {
            currentButton.color = Color.gray;
        }
        _quickButtonList[i].color = Color.white;

        _panelParent.GetComponent<RectTransform>().DOAnchorPosX(target * -page, duration).SetUpdate(true)
            .OnComplete(() => _isTweening = false);
    }
}