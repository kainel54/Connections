using DG.Tweening;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;

public class TitleFireSound : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundChannel;
    
    [SerializeField] private SoundSO _enterFireSound;
    [SerializeField] private SoundSO _fireBGMSound;
    [SerializeField] private SoundSO _titleBGMSound;

    private void Start()
    {
        var playFireSound = SoundEvents.PlaySfxEvent;
        playFireSound.position = transform.position;
        playFireSound.clipData = _enterFireSound;
        
        _soundChannel.RaiseEvent(playFireSound);

        DOVirtual.DelayedCall(_enterFireSound.clip.length + 0.05f, PlayBGMSound);
    }

    private void PlayBGMSound()
    {
        var playFireSound = SoundEvents.PlayBGMEvent;
        playFireSound.clipData = _fireBGMSound;
        _soundChannel.RaiseEvent(playFireSound);
        
        var playTitleSound = SoundEvents.PlayBGMEvent;
        playTitleSound.clipData = _titleBGMSound;
        _soundChannel.RaiseEvent(playTitleSound);
    }
}
