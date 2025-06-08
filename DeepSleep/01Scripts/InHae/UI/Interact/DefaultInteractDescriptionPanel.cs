using IH.EventSystem.InteractEvent;
using TMPro;
using UnityEngine;

public class DefaultInteractDescriptionPanel : BaseInteractDescription
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    
    private void Awake()
    {
        _interactEventChannel.AddListener<DefaultInteractDescriptionEvent>(HandleDefaultInteractDescription);
    }

    private void OnDestroy()
    {
        _interactEventChannel.RemoveListener<DefaultInteractDescriptionEvent>(HandleDefaultInteractDescription);
    }
    
    private void HandleDefaultInteractDescription(DefaultInteractDescriptionEvent evt)
    {
        if (evt.isPanelActive)
        {
            _titleText.text = evt.title;
            _descriptionText.text = evt.description;
            
            ShowPanel(evt.position, evt.yOffset);
        }
        else
        {
            HidePanel();
        }
    }
}
