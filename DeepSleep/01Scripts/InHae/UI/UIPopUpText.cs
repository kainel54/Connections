using DG.Tweening;
using ObjectPooling;
using System;
using TMPro;
using UnityEngine;

public class UIPopUpText : MonoBehaviour, IPoolable
{
    private TextMeshProUGUI _text;
    
    private RectTransform _rectTransform => transform as RectTransform;
    public GameObject GameObject { get => gameObject; set { } }

    [SerializeField] private UIPoolingType _type;
    public Enum PoolEnum { get => _type; set { } }
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void TextInit(string text, float fontSize, Color color, Vector3 position)
    {
        transform.localScale = Vector3.one;
        
        _text.text = text;
        _text.fontSize = fontSize;
        _text.color = color;
        _rectTransform.position = position;
    }

    public void UpAndFadeText(float yDelta = 2f, float duration = 0.5f)
    {
        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Join(_text.DOFade(0, duration));
        seq.Join(_rectTransform.DOAnchorPosY( _rectTransform.position.y + yDelta, duration));
        seq.OnComplete(() => PoolManager.Instance.Push(this, true));
    }
    
    public void Init()
    {
    }

    public void OnPop()
    {

    }

    public void OnPush()
    {

    }
}
