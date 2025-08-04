using IH.EventSystem.LevelEvent;
using System.Collections.Generic;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;

public class TotemManager : MonoBehaviour
{
    [SerializeField] private List<Totem> _totemList = new List<Totem>();
    [SerializeField] private GameEventChannelSO _destroyTotemEvent;
    
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _raiseSoundSO;
    [SerializeField] private SoundSO _lowerSoundSO;

    private void OnEnable()
    {
        _destroyTotemEvent.AddListener<DestroyTotemEvent>(LowerTotems);
    }

    private void Start()
    {
        Totem[] children = GetComponentsInChildren<Totem>();

        foreach (var child in children)
            _totemList.Add(child);
    }

    public void RaiseTotems()
    {
        PlaySound(_raiseSoundSO);
        foreach (Totem totem in _totemList)
        {
            totem.RaiseTotem();
        }
    }

    private void LowerTotems(DestroyTotemEvent evt)
    {
        PlaySound(_lowerSoundSO);
        foreach (Totem totem in _totemList)
        {
            totem.LowerTotem();
        }

        _totemList.Clear();
    }

    private void OnDisable()
    {
        _destroyTotemEvent.RemoveListener<DestroyTotemEvent>(LowerTotems);
    }
    
    private void PlaySound(SoundSO sound)
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.position = transform.position;
        soundEvt.clipData = sound;
        _soundEventChannel.RaiseEvent(soundEvt);
    }
}
