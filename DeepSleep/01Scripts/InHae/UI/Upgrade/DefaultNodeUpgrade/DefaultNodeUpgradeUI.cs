using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using UnityEngine;
using YH.EventSystem;

public class DefaultNodeUpgradeUI : NodeUpgradeUIBase
{
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannel;
    
    public override void OpenWindow()
    {
        base.OpenWindow();
        
        var upgradeSkillInitEvent = DefaultNodeUpgradeEvents.UpgradeSkillInitEvent;
        _defaultNodeEventChannel.RaiseEvent(upgradeSkillInitEvent);
    }
}
