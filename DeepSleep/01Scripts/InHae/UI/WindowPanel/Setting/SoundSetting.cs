using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _bgmSlider;

    private void Start()
    {
        float t = Mathf.InverseLerp(-60f, 0f, GetAudioValue("SFX"));
        _sfxSlider.value = Mathf.Lerp(0.001f, 1, t);

        t = Mathf.InverseLerp(-60f, 0f, GetAudioValue("BGM"));
        _bgmSlider.value = Mathf.Lerp(0.001f, 1, t);

        _sfxSlider.onValueChanged.AddListener(HandleSfxValueChange);
        _bgmSlider.onValueChanged.AddListener(HandleBGMValueChange);
    }

    private void HandleBGMValueChange(float value)
    {
        SetAudioValue("BGM", value);
    }

    private void HandleSfxValueChange(float value)
    {
        SetAudioValue("SFX", value);
    }
    
    private void SetAudioValue(string audioType, float value)
    {
        _audioMixer.SetFloat(audioType, Mathf.Log10(value) * 20);
    }
    
    private float GetAudioValue(string audioType)
    {
        _audioMixer.GetFloat(audioType, out float v);
        return v;
    }
}
