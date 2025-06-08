using System;
using System.Collections;
using IH.EventSystem.SoundEvent;
using IH.EventSystem.SystemEvent;
using UnityEngine;
using YH.EventSystem;

public class LevelGenerateComplete : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannel;
    [SerializeField] private GameEventChannelSO _soundChannel;

    private LevelGenerator _levelGenerator;

    private void Awake()
    {
        _levelGenerator = GetComponent<LevelGenerator>();
        _levelGenerator.GenerateCompleteAction += HandleFadeIn;
    }

    private void Start()
    {
        var evt = SystemEvents.FirstFadeSetting;
        _systemChannel.RaiseEvent(evt);
    }

    private void OnDestroy()
    {
        _levelGenerator.GenerateCompleteAction -= HandleFadeIn;
    }

    private void HandleFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        var fadeOutEvent = SystemEvents.FadeScreenEvent;
        fadeOutEvent.isFadeIn = true;
        fadeOutEvent.isCircle = true;
        fadeOutEvent.fadeDuration = 0.5f;
        
        _systemChannel.AddListener<FadeComplete>(MusicPlay);

        yield return new WaitForSeconds(0.5f);
        _systemChannel.RaiseEvent(fadeOutEvent);
    }

    private void MusicPlay(FadeComplete evt)
    {
        _systemChannel.RemoveListener<FadeComplete>(MusicPlay);
        
        var soundEvt = SoundEvents.PlayBGMEvent;
        soundEvt.clipData = _levelGenerator.stageDataSO.DefaultSoundSo;
        _soundChannel.RaiseEvent(soundEvt);
    }
}
