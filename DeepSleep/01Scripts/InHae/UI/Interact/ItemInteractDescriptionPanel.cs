using System.Text;
using IH.EventSystem.InteractEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractDescriptionPanel : BaseInteractDescription
{
    [SerializeField] private float _yOffset;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _tierAndTypeText;
    [SerializeField] private TextMeshProUGUI _defaultDescriptionText;
    [SerializeField] private Image _iconImage;
    
    private SkillResultDescription _skillDescription;
    
    private StringBuilder _tierAndTypeTextBuilder;
    
    private void Awake()
    {
        _tierAndTypeTextBuilder = new StringBuilder();
        
        _interactEventChannel.AddListener<SkillInteractDescriptionPanelEvent>(HandleSkillDescription);
        _skillDescription = GetComponentInChildren<SkillResultDescription>();
    }

    private void OnDestroy()
    {
        _interactEventChannel.RemoveListener<SkillInteractDescriptionPanelEvent>(HandleSkillDescription);
    }

    private void HandleSkillDescription(SkillInteractDescriptionPanelEvent evt)
    {
        if (evt.isPanelActive)
        {
            _tierAndTypeTextBuilder.Clear();
            
            ItemTier tier = evt.itemDataSo.itemTier;
            Color tierColor = EnumColorManager.Instance.GetColor(tier);
            
            _titleText.text = evt.itemDataSo.itemName;
            _titleText.color = tierColor;

            _tierAndTypeTextBuilder.Append($"<color=#{ColorUtility.ToHtmlStringRGB(tierColor)}>");
            _tierAndTypeTextBuilder.Append($"[{EnumStringManager.Instance.GetString(tier)}]</color>  ");
            _tierAndTypeTextBuilder.Append($"[{EnumStringManager.Instance.GetString(evt.itemDataSo.itemType)}]");
            
            _tierAndTypeText.text = _tierAndTypeTextBuilder.ToString();
            
            _iconImage.sprite = evt.itemDataSo.icon;
            ShowDescription(evt.itemDataSo);
            
            ShowPanel(evt.position, _yOffset);
        }
        else
        {
            HidePanel();
        }
    }

    private void ShowDescription(ItemDataSO itemDataSo)
    {
        if (itemDataSo is SkillItemSO skillItemSo)
        {
            _defaultDescriptionText.gameObject.SetActive(false);
            _skillDescription.gameObject.SetActive(true);
                
            _skillDescription.ResultDescription(skillItemSo,
                SkillManager.Instance.GetSkill(skillItemSo.reflectionName));
        }
        else
        {
            _defaultDescriptionText.gameObject.SetActive(true);
            _skillDescription.gameObject.SetActive(false);

            _defaultDescriptionText.text = itemDataSo.itemDescription;
        }
    }
}
