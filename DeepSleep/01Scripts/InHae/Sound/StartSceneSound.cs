using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;

public class StartSceneSound : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundChannel;

    [SerializeField] private SoundSO _titleBgm;
    [SerializeField] private SoundSO _helpOpenAndNextSound;
    [SerializeField] private SoundSO _helpCloseSound;
    [SerializeField] private SoundSO _buttonClickSound;

    private void Start()
    {
        var evt = SoundEvents.PlayBGMEvent;
        evt.clipData = _titleBgm;
        _soundChannel.RaiseEvent(evt);   
    }

    public void HelpOpenAndNextSound()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _helpOpenAndNextSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
    }
    
    public void HelpCloseSound()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _helpCloseSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
    }
    
    public void HandleButtonClickSound()
    {
        var evt = SoundEvents.PlaySfxEvent;
        evt.clipData = _buttonClickSound;
        evt.position = transform.position;

        _soundChannel.RaiseEvent(evt); 
    }
}
