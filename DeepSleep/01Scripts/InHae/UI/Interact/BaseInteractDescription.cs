using DG.Tweening;
using UnityEngine;
using YH.EventSystem;

public abstract class BaseInteractDescription : MonoBehaviour
{
    [SerializeField] protected GameEventChannelSO _interactEventChannel;
    [SerializeField] protected float _scale;
    
    protected RectTransform _rectTransform => transform as RectTransform;

    protected void ShowPanel(Vector3 pos, float yOffset)
    {
        Vector3 position = Camera.main.WorldToScreenPoint(pos);
        position.y += yOffset;
        _rectTransform.anchoredPosition = position;

        _rectTransform.DOScale(Vector3.one * _scale, 0.3f);
    }
    
    protected void HidePanel() => _rectTransform.DOScale(Vector3.zero, 0.3f);
}
