using IH.EventSystem.InteractEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInteractDescriptionPanel : BaseInteractDescription
{
    [SerializeField] private float _yOffset;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Image _iconImage;
    
    private void Awake()
    {
        _interactEventChannel.AddListener<SkillInteractDescriptionPanelEvent>(HandleSkillDescription);
    }

    private void OnDestroy()
    {
        _interactEventChannel.RemoveListener<SkillInteractDescriptionPanelEvent>(HandleSkillDescription);
    }

    private void HandleSkillDescription(SkillInteractDescriptionPanelEvent evt)
    {
        if (evt.isPanelActive)
        {
            _titleText.text = evt.itemDataSo.itemName;
            _iconImage.sprite = evt.itemDataSo.icon;
            
            ShowPanel(evt.position, _yOffset);
        }
        else
        {
            HidePanel();
        }
    }
}
