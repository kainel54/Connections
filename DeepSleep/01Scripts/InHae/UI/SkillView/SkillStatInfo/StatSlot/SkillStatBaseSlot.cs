using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SkillStatBaseSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillStatInfoSO statInfo;
    [SerializeField] protected Image _iconImage;
    [SerializeField] protected TextMeshProUGUI _statNameText;
    [SerializeField] protected TextMeshProUGUI _valueText;
    [SerializeField] private float _disableAlpha;
    
    private SkillStatPopUpUI _skillStatPopUp;
    private RectTransform _popUpPanelRectTransform => _skillStatPopUp.transform as RectTransform;
    
    private Color _imageDefaultColor;
    private Color _textDefaultColor;

    private void Start()
    {
        _skillStatPopUp = UIHelper.Instance.GetSkillStatPopUpUI();
        
        _imageDefaultColor = _iconImage.color;
        _textDefaultColor = _valueText.color;
    }

    public virtual void Init(BaseSkillStatElement baseSkillStatElement)
    {
        _statNameText.text = baseSkillStatElement.statInfo.title;
        
        _iconImage.color = _imageDefaultColor;
        _valueText.color = _textDefaultColor;
        _statNameText.color = _textDefaultColor;
        
        _iconImage.sprite = baseSkillStatElement.statInfo.icon;
        _iconImage.color = baseSkillStatElement.statInfo.iconColor;
    }

    public void Disable()
    {
        _valueText.color = new Color(1, 1, 1, _disableAlpha);
        _statNameText.color = new Color(1, 1, 1, _disableAlpha);
        
        Color iconColor = _iconImage.color;
        iconColor.a = _disableAlpha;
        _iconImage.color = iconColor;
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
