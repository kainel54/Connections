using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SkillStatBaseSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillStatInfoSO statInfo;
    [SerializeField] protected Image _iconImage;
    [SerializeField] protected TextMeshProUGUI _valueText;
    private Image _image;
    
    private SkillStatPopUpUI _skillStatPopUp;
    private RectTransform _popUpPanelRectTransform => _skillStatPopUp.transform as RectTransform;
    
    private Color _defaultColor;
    private Color _imageDefaultColor;
    private Color _textDefaultColor;

    private void Start()
    {
        _image = GetComponent<Image>();
        _skillStatPopUp = UIHelper.Instance.GetSkillStatPopUpUI();
        
        _defaultColor = _image.color;
        _imageDefaultColor = _iconImage.color;
        _textDefaultColor = _valueText.color;
    }

    public virtual void Init(BaseSkillStatElement baseSkillStatElement)
    {
        _image.color = _defaultColor;
        _iconImage.color = _imageDefaultColor;
        _valueText.color = _textDefaultColor;
        
        _iconImage.sprite = baseSkillStatElement.statInfo.icon;
    }

    public void Disable()
    {
        _image.color = new Color(1, 1, 1, 0.1f);
        _iconImage.color = new Color(1, 1, 1, 0.1f);
        _valueText.color = new Color(1, 1, 1, 0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.y += _popUpPanelRectTransform.sizeDelta.y * 0.5f;
        _skillStatPopUp.transform.position = pos;
        
        _skillStatPopUp.Init(statInfo);
        _skillStatPopUp.OnPopUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _skillStatPopUp.EndPopUp();
    }
}
