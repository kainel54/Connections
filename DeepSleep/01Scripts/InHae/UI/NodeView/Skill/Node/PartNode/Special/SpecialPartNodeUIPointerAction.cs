using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class SpecialPartNodeUIPointerAction : PartNodeUIPointerAction
{
    private SpecialPartNodeUI _specialPartNodeUI;
    private NodeAbilityInventoryItem _currentAbility => _specialPartNodeUI.CurrentEquipData.nodeAbilityInventoryItem;
    private PartInventoryItem _currentPart => _specialPartNodeUI.CurrentEquipData.partInventoryItem;
    
    private bool _abilityEmpty => _currentAbility == null || _currentAbility.data == null;
    
    public override void Initialize(PartNodeUI partNodeUI)
    {
        base.Initialize(partNodeUI);
        _specialPartNodeUI = _partNodeUI as SpecialPartNodeUI;
        _onPopUpCompo = _partNodeUI.GetCompo<SpecialPartNodeUIOnPopUp>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if(_specialPartNodeUI.isSpecialMode && _abilityEmpty)
            return;
        if(!_specialPartNodeUI.isSpecialMode && _specialPartNodeUI.isPartEmpty)
            return;
        
        InventoryItem item = _specialPartNodeUI.isSpecialMode ? _currentAbility : _currentPart;
        if (item == null)
            return;
        
        _partNodeUI.UpdateSlotImage(null, Color.clear);        
        
        _dragItem.StartDrag(item);
        _dragTarget = _dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;
        
        _specialPartNodeUI.ShowTransitionGuide(false);
    }

    public override void OnDrag(PointerEventData eventData)
    {       
        if(_specialPartNodeUI.isSpecialMode && _abilityEmpty)
            return;
        if(!_specialPartNodeUI.isSpecialMode && _specialPartNodeUI.isPartEmpty)
            return;
        
        _dragTarget.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(!_dragItem.isDragging)
            return;
        _dragItem.EndDrag();

        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        if(_specialPartNodeUI.isSpecialMode && !_specialPartNodeUI.isAbilityEmpty)
            _partNodeUI.UpdateSlotImage(_currentAbility.data.icon, Color.white);
        if(!_specialPartNodeUI.isSpecialMode && !_specialPartNodeUI.isPartEmpty)
            _partNodeUI.UpdateSlotImage(_currentPart.data.icon, Color.white);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        var dragTarget = eventData.pointerDrag;
        if(_specialPartNodeUI.isEquipDataEmpty || dragTarget == gameObject)
            return;
        if(_specialPartNodeUI.isSpecialMode && _abilityEmpty)
            return;
        
        if (!_abilityEmpty)
            _specialPartNodeUI.ShowTransitionGuide(true);
        
        InventoryItem item = _specialPartNodeUI.isSpecialMode ? _currentAbility : _currentPart;
        _onPopUpCompo.CurrentPopUpOn(item);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_specialPartNodeUI.isEquipDataEmpty)
            return;
        
        _onPopUpCompo.EndPopUp();
        _specialPartNodeUI.ShowTransitionGuide(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;
        
        if (_specialPartNodeUI.isSpecialMode)
        {
            if(_abilityEmpty)
                return;
        
            _specialPartNodeUI.ReturnNodeAbility();
            _onPopUpCompo.EndPopUp();
        }
        else
        {
            base.OnPointerClick(eventData);
        }
    }
}
