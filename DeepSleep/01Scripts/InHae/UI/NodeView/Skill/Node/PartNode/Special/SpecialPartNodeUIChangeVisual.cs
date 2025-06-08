using System;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YH.EventSystem;

public class SpecialPartNodeUIChangeVisual : MonoBehaviour, IPartNodeUIComponent, IPointerClickHandler
{
    [SerializeField] private GameEventChannelSO _specialPartNodeEventChannel;
    private SpecialPartNodeUI _specialPartNodeUI;
    private NodeAbilityInventoryItem _currentAbility => _specialPartNodeUI.CurrentEquipData.nodeAbilityInventoryItem;
    private PartInventoryItem _currentPart => _specialPartNodeUI.CurrentEquipData.partInventoryItem;
    private bool _isNodeAbilityEmpty => _currentAbility == null || _currentAbility.data == null;

    private SpecialPartNodeUIOnPopUp _specialPartNodeUIOnPopUp;

    
    public void Initialize(PartNodeUI partNodeUI)
    {
        _specialPartNodeUI = partNodeUI as SpecialPartNodeUI;
        _specialPartNodeUI.SpecialModeChangedAction += HandleChangeVisual;
        
        _specialPartNodeUIOnPopUp = _specialPartNodeUI.GetCompo<SpecialPartNodeUIOnPopUp>();
    }

    private void OnDestroy()
    {
        _specialPartNodeUI.SpecialModeChangedAction -= HandleChangeVisual;
    }

    private void HandleChangeVisual(bool isSpecialMode)
    {
        if (_specialPartNodeUI.isSpecialMode)
        {
            if (_specialPartNodeUI.CurrentEquipData == null || _isNodeAbilityEmpty)
                _specialPartNodeUI.UpdateSlotImage(null, _specialPartNodeUI.specialSlotColor);
            else
                _specialPartNodeUI.UpdateSlotImage(_currentAbility.data.icon, Color.white, true);
        }
        else
        {
            if(_specialPartNodeUI.isPartEmpty)
                _specialPartNodeUI.UpdateSlotImage(null, Color.clear, true);
            else
                _specialPartNodeUI.UpdateSlotImage(_currentPart.data.icon, Color.white, true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging || eventData.button != PointerEventData.InputButton.Left)
            return;
        if(_specialPartNodeUI.isEquipDataEmpty || _isNodeAbilityEmpty)
            return;

        var specialModeChangeEvt = SpecialPartNodeEvents.ChangeSpecialModeEvent;
        _specialPartNodeEventChannel.RaiseEvent(specialModeChangeEvt);
        
        InventoryItem item = _specialPartNodeUI.isSpecialMode ? _currentAbility : _currentPart;
        _specialPartNodeUIOnPopUp.CurrentPopUpOn(item);
    }
}
