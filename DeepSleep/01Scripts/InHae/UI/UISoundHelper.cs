using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;

public class UISoundHelper : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundChannelSO;
    [SerializeField] private SoundSO _buttonClickSound;

    public void ButtonClickSound()
    {
        var soundPlayEvt = SoundEvents.PlaySfxEvent;
        soundPlayEvt.clipData = _buttonClickSound;
        soundPlayEvt.position = transform.position;
        _soundChannelSO.RaiseEvent(soundPlayEvt);
    }
}
