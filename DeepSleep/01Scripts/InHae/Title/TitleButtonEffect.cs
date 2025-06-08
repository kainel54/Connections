using DG.Tweening;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class TitleButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameEventChannelSO _soundChannel;
    [SerializeField] private SoundSO _buttonOnSound;
    
    [SerializeField] private UnityEvent _clickEvent;
    [SerializeField] private float _tweenTime;
    [SerializeField] private float _shakeTime;
    [SerializeField] private float _shakeValue;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _buttonOnSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
        
        transform.DOScale(Vector3.one * 1.3f, _tweenTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, _tweenTime);
    }

    public void ClickEffect()
    {
        transform.DOShakePosition(_shakeTime, _shakeValue);
        _clickEvent?.Invoke();
    }
}
