using DG.Tweening;
using IH.EventSystem.VolumeEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Feedbacks;

public class BloodScreenFeedback : Feedback
{
    [SerializeField] private GameEventChannelSO _volumeEventChannel;
    
    [SerializeField] private float _duration;
    [SerializeField] private float _intensity;
    [SerializeField] private float _lerpTime;
    
    public override void CreateFeedback()
    {
        var evt = VolumeEvents.VignetteSettingEvent;
        evt.color = new Color(0.7f, 0, 0);
        evt.intensity = _intensity;
        evt.lerpTime = _lerpTime;

        _volumeEventChannel.RaiseEvent(evt);
        
        DOVirtual.DelayedCall(_duration, () => _volumeEventChannel.RaiseEvent(VolumeEvents.VignetteResetEvent));
    }

    public override void FinishFeedback()
    {
        _volumeEventChannel.RaiseEvent(VolumeEvents.VignetteResetEvent);
    }
}
