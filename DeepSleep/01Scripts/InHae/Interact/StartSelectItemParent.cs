using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;

public class StartSelectItemParent : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _chainUpSoundSo;
    
    [SerializeField] private StartItemListSO _startItemListSO;
    
    private List<StartSelectItem> _startSelectItems = new List<StartSelectItem>();

    private bool _isStopSound;

    private void Awake()
    {
        _startSelectItems = GetComponentsInChildren<StartSelectItem>().ToList();
        _startSelectItems.ForEach(x =>
        {
            x.SpecialInit(_startItemListSO.GetRandomSkillItem());
            x.VisualInit();

            x.HandleInteractAction += HandleGetCheck;
        });
    }

    private void HandleGetCheck()
    {
        StartChainMoveSound();
        foreach (var selectItem in _startSelectItems)
        {
            if(selectItem.isCollected)
                continue;
            
            selectItem.NoSelectable();
        }
    }

    private void StartChainMoveSound()
    {
        var playSoundEvt = SoundEvents.PlaySfxEvent;
        playSoundEvt.clipData = _chainUpSoundSo;
        playSoundEvt.position = transform.position;

        _soundEventChannel.RaiseEvent(playSoundEvt);
    }
}
