using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using YH.EventSystem;

public class SpecialNodeUpgradeUI : NodeUpgradeUIBase
{
    [SerializeField] private GameEventChannelSO _specialNodeUpgradeEventChannel;
    
    public override void OpenWindow()
    {
        base.OpenWindow();

        var evt = SpecialNodeUpgradeEvents.UpgradeSkillInitEvent;
        _specialNodeUpgradeEventChannel.RaiseEvent(evt);
    }
}
