using IH.EventSystem.InteractEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using UnityEngine;
using YH.EventSystem;

public class UpgradeArea : Interactable
{
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    [SerializeField] private GameEventChannelSO _interactEventChannel;
    [SerializeField] private float _yOffset;
    
    public override void EnableDescription()
    {
        var showDescriptionPanelEvent = InteractEvents.DefaultInteractDescriptionEvent;
        showDescriptionPanelEvent.isPanelActive = true;
        showDescriptionPanelEvent.position = transform.position;
        showDescriptionPanelEvent.title = "용광로";
        showDescriptionPanelEvent.description = "[F] 키를 눌러서 강화하기";
        showDescriptionPanelEvent.yOffset = _yOffset;
        
        _interactEventChannel.RaiseEvent(showDescriptionPanelEvent);
    }

    public override void DisableDescription()
    {
        var showDescriptionPanelEvent = InteractEvents.DefaultInteractDescriptionEvent;
        showDescriptionPanelEvent.isPanelActive = false;
        _interactEventChannel.RaiseEvent(showDescriptionPanelEvent);
    }

    public override void Interact()
    {
        base.Interact();
        
        var evt = UIPanelEvent.UpgradePanelEvent;
        evt.isPanelActive = true;
        _uiEventChannel.RaiseEvent(evt);
    }
}
