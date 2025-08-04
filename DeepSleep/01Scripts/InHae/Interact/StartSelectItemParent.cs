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
    
    private List<StartSelectItem> _startSelectItems = new();

    private bool _isStopSound;

    private void Awake()
    {
        _startSelectItems = GetComponentsInChildren<StartSelectItem>().ToList();

        List<SkillItemSO> randItems = _startItemListSO.GetRandomNoDuplicationSkillItems(4);

        for (int i = 0; i < randItems.Count; i++)
        {
            _startSelectItems[i].SpecialInit(randItems[i]);
            _startSelectItems[i].VisualInit();

            _startSelectItems[i].HandleInteractAction += HandleGetCheck;
        }
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
