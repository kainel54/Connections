using UnityEngine;
using YH.EventSystem;

public class UpgradeArea : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var evt = UIEvents.UpgradePanelEvent;
            evt.isPanelActive = true;
            _uiEventChannel.RaiseEvent(evt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var evt = UIEvents.UpgradePanelEvent;
            evt.isPanelActive = false;
            _uiEventChannel.RaiseEvent(evt);
        }
    }
}
