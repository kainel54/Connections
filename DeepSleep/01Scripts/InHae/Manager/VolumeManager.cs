using DG.Tweening;
using IH.EventSystem.VolumeEvent;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using YH.EventSystem;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _volumeEventChannel;

    private Volume _volume;

    private bool _isBlooding;
    private Color _vignetteDefaultColor;
    private float _defaultVignetteIntensity;
    
    private void Awake()
    {
        _volume = GetComponent<Volume>();

        if (_volume.profile.TryGet(out Vignette vignette))
        {
            _defaultVignetteIntensity = vignette.intensity.value;
            _vignetteDefaultColor = vignette.color.value;
        }

        _volumeEventChannel.AddListener<VignetteSetting>(HandleVignetteSetting);
        _volumeEventChannel.AddListener<VignetteReset>(HandleVignetteReset);
    }

    private void OnDestroy()
    {
        _volumeEventChannel.RemoveListener<VignetteSetting>(HandleVignetteSetting);
        _volumeEventChannel.RemoveListener<VignetteReset>(HandleVignetteReset);
    }

    private void HandleVignetteReset(VignetteReset evt)
    {
        _isBlooding = false;
        if (_volume.profile.TryGet(out Vignette vignette))
        {
            DOTween.To(() => vignette.intensity.value, f => vignette.intensity.value = f,
                _defaultVignetteIntensity, 0.2f).OnComplete(() =>
            {
                if(_isBlooding)
                    return;
                
                vignette.color.value = _vignetteDefaultColor;
            });
        }
    }

    private void HandleVignetteSetting(VignetteSetting evt)
    {
        if (_isBlooding)
            return;

        _isBlooding = true;
        if (_volume.profile.TryGet(out Vignette vignette))
        {
            vignette.color.value = evt.color;
            DOTween.To(() => vignette.intensity.value, f => vignette.intensity.value = f
                , evt.intensity, evt.lerpTime);
        }
    }
}
