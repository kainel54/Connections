using UnityEngine;

public class SpecialPartNodeUIEquipProcess : MonoBehaviour, IPartNodeUIComponent
{
    private PartNodeUI _partNodeUI;
    private NodeAbilityInventoryItem _currentInventoryItem => _partNodeUI.CurrentEquipData.nodeAbilityInventoryItem;
    private bool _isEmpty => _currentInventoryItem == null;
    
    public void Initialize(PartNodeUI partNodeUI)
    {
        _partNodeUI = partNodeUI;
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
    
    private void HandleEquipNode()
    {
        if(_isEmpty)
            return;
        
        foreach (var connectedNode in _partNodeUI.connectedNodes)
        {
            bool isValidTarget = false;

            if (connectedNode is PartNodeUI partNodeUI && !partNodeUI.isPartEmpty)
                isValidTarget = true;
            else if (connectedNode is SkillNodeUI)
                isValidTarget = true;

            if (isValidTarget)
            {
                _currentInventoryItem.nodeAbility
                    .ApplyAbility(_partNodeUI.index, connectedNode.index, _partNodeUI.currentSkill);
            }
        }
    }

    public void HandleUnEquipNode()
    {
        if(_isEmpty)
            return;
        
        foreach (var connectedNode in _partNodeUI.connectedNodes)
        {
            bool isValidTarget = false;

            if (connectedNode is PartNodeUI partNodeUI && !partNodeUI.isPartEmpty)
                isValidTarget = true;
            else if (connectedNode is SkillNodeUI)
                isValidTarget = true;

            if (isValidTarget)
            {
                _currentInventoryItem.nodeAbility
                    .UnApplyAbility(_partNodeUI.index, connectedNode.index, _partNodeUI.currentSkill);
            }
        }
    }

    private void HandleInitNode()
    {
        if(_isEmpty)
            return;
        
        _currentInventoryItem.nodeAbility.InitAbility(_partNodeUI.index, _partNodeUI.currentSkill);
    }
}
