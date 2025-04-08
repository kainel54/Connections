using DG.Tweening;
using IH.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class FadeCanvas : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannel;
    [SerializeField] private Image _fadeImage;

    private readonly int _circleValue = Shader.PropertyToID("_CircleValue");
    private readonly int _fadeValue = Shader.PropertyToID("_FadeValue");
    private readonly int _isCircleHash = Shader.PropertyToID("_IsCircle");

    private void Awake()
    {
        _fadeImage.material = new Material(_fadeImage.material);
        _systemChannel.AddListener<FadeScreenEvent>(HandleFadeScreen);
        _systemChannel.AddListener<FirstFadeSetting>(HandleFirstFadeSetting);
    }
    

    private void OnDestroy()
    {
        _systemChannel.RemoveListener<FadeScreenEvent>(HandleFadeScreen);
        _systemChannel.RemoveListener<FirstFadeSetting>(HandleFirstFadeSetting);
    }

    private void HandleFadeScreen(FadeScreenEvent evt)
    {
        int isCircle = evt.isCircle ? 1 : 0;
        _fadeImage.material.SetInt(_isCircleHash, isCircle);

        if (evt.isCircle)
            CircleType(evt);
        else
            DefaultType(evt);
    }

    private void CircleType(FadeScreenEvent evt)
    {
        float fadeValue = evt.isFadeIn ? 2.5f : 0f;
        float startValue = evt.isFadeIn ? 0f : 2.5f;
        _fadeImage.material.SetFloat(_circleValue, startValue);

        _fadeImage.material.DOFloat(fadeValue, _circleValue, evt.fadeDuration).OnComplete(() =>
        {
            _systemChannel.RaiseEvent(SystemEvents.FadeComplete);
        });
    }

    private void DefaultType(FadeScreenEvent evt)
    {
        float fadeValue = evt.isFadeIn ? 0f : 1f;
        float startValue = evt.isFadeIn ? 1f : 0f;
        _fadeImage.material.SetFloat(_fadeValue, startValue);

        _fadeImage.material.DOFloat(fadeValue, _fadeValue, evt.fadeDuration).OnComplete(() =>
        {
            _systemChannel.RaiseEvent(SystemEvents.FadeComplete);
        });
    }
    
    private void HandleFirstFadeSetting(FirstFadeSetting evt)
    {
        _fadeImage.material.SetFloat(_fadeValue, 1f);
        _fadeImage.material.SetFloat(_circleValue, 2.5f);
    }
}
