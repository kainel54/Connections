using UnityEngine;

public class PartNodeUIEquipProcess : MonoBehaviour, IPartNodeUIComponent
{
    private PartNodeUI _partNodeUI;
    private PartNodeUIChainCheck _uiChainCheck;

    private PartNode _currentPart => _uiChainCheck.isChained ? _uiChainCheck.chainPartNode : 
        _partNodeUI.CurrentEquipData.partInventoryItem.partNode;
    
    public void Initialize(PartNodeUI partNodeUI)
    {
        _partNodeUI = partNodeUI;
        _uiChainCheck = _partNodeUI.GetCompo<PartNodeUIChainCheck>();
        
        _partNodeUI.EquipNodeAction += HandleEquipNode;
        _partNodeUI.UnEquipNodeAction += HandleUnEquipNode;
        _partNodeUI.InitNodeAction += HandleInitNode;
    }
    
    private void OnDestroy()
    {
        _partNodeUI.EquipNodeAction -= HandleEquipNode;
        _partNodeUI.UnEquipNodeAction -= HandleUnEquipNode;
        _partNodeUI.InitNodeAction -= HandleInitNode;
    }
    
    private void HandleInitNode()
    {
        _currentPart.InitPart(_partNodeUI.currentSkill);
    }

    private void HandleUnEquipNode()
    {
        _currentPart.UnEquipPart(_partNodeUI.currentSkill);
    }

    private void HandleEquipNode()
    {
        _currentPart.EquipPart(_partNodeUI.currentSkill);
    }
}
