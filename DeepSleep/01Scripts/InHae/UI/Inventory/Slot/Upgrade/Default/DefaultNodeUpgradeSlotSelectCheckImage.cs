using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;

public class DefaultNodeUpgradeSlotSelectCheckImage : NodeUpgradeSlotSelectCheckImage
{
    private void Awake()
    {
        _upgradeEventChannel.AddListener<UpgradeSlotSelectImageEvent>(HandleUpgradeSkillSelect);
        _upgradeEventChannel.AddListener<UpgradeSkillInitEvent>(HandleImageDisable);
        
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        _upgradeEventChannel.RemoveListener<UpgradeSlotSelectImageEvent>(HandleUpgradeSkillSelect);        
        _upgradeEventChannel.RemoveListener<UpgradeSkillInitEvent>(HandleImageDisable);
    }
    
    private void HandleUpgradeSkillSelect(UpgradeSlotSelectImageEvent evt)
    {
        Init(evt.targetTrm);
    }
    
    private void HandleImageDisable(UpgradeSkillInitEvent evt)
    {
        gameObject.SetActive(false);
    }
}
