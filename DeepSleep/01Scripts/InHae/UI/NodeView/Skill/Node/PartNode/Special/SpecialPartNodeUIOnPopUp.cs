using UnityEngine;

public class SpecialPartNodeUIOnPopUp : PartNodeUIOnPopUp
{
    private SpecialPartNodeUI _specialPartNodeUI;
    private NodeAbilityInventoryItem _currentAbility => _specialPartNodeUI.CurrentEquipData.nodeAbilityInventoryItem;
    private PartInventoryItem _currentPart => _specialPartNodeUI.CurrentEquipData.partInventoryItem;

    public override void Initialize(PartNodeUI partNodeUI)
    {
        _specialPartNodeUI = partNodeUI as SpecialPartNodeUI;
        _specialPartNodeUI.SpecialModeChangedAction += HandleChangePanel;
    }

    private void OnDestroy()
    {
        _specialPartNodeUI.SpecialModeChangedAction -= HandleChangePanel;
    }

    private void HandleChangePanel(bool isSpecialMode)
    {
        if(_specialPartNodeUI.isPartEmpty || _specialPartNodeUI.isAbilityEmpty)
            return;
           
        Vector3 beforePos = _popUpPanel.transform.localPosition;

        PopUpChange(_specialPartNodeUI.isSpecialMode);
        _popUpPanel.transform.localPosition = beforePos;
    }
    
    public void PopUpChange(bool isSpecialMode)
    {
        _popUpPanel.EndPopUp();
        _popUpPanel = UIHelper.Instance.GetPopUpPanel(isSpecialMode ? 
            ItemPopUpItemType.NodeAbility : ItemPopUpItemType.Part);
    }
}
