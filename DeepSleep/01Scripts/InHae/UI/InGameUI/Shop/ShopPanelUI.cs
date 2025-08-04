using DG.Tweening;
using IH.EventSystem.SoundEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YH.EventSystem;

public class ShopPanelUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _buySoundSo;
    [SerializeField] private SoundSO _cantBuySoundSo;
    
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button _buyButton;
    
    [SerializeField] private TextMeshProUGUI _defaultDescriptionText;
    private SkillResultDescription _skillDescription;
    private RectTransform _rectTransform => transform as RectTransform;
    
    private void Awake()
    {
        _uiEventChannel.AddListener<ShopDescriptionPanelEvent>(HandleShopDescription);
        _skillDescription = GetComponentInChildren<SkillResultDescription>();
    }

    private void OnDestroy()
    {
        _uiEventChannel.RemoveListener<ShopDescriptionPanelEvent>(HandleShopDescription);
    }

    private void HandleShopDescription(ShopDescriptionPanelEvent evt)
    {
        if (evt.isPanelActive)
        {
            _titleText.text = evt.itemDataSo.itemName;
            _priceText.text = evt.itemDataSo.price + "$";  
            _iconImage.sprite = evt.itemDataSo.icon;

            _titleText.color = evt.textColor;
            
            if (evt.itemDataSo is SkillItemSO skillItemSo)
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

                _defaultDescriptionText.text = evt.itemDataSo.itemDescription;
                _defaultDescriptionText.color = evt.textColor;
            }
            
            _priceText.color = evt.canBuyItem ? Color.yellow : Color.red;

            if (evt.canBuyItem)
            {
                _buyButton.onClick.AddListener(evt.buyItemAction.Invoke);
                _buyButton.onClick.AddListener(CanBuySound);
            }
            else
            {
                _buyButton.onClick.AddListener(CantButItem);
            }
            
            _rectTransform.DOScale(Vector3.one, 0.3f);
        }
        else
        {
            _buyButton.onClick.RemoveAllListeners();
            _rectTransform.DOScale(Vector3.zero, 0.3f);
        }
    }

    private void CantText()
    {
        UIPopUpText uiPopUp = PoolManager.Instance.Pop(UIPoolingType.UIPopUpText) as UIPopUpText;
        uiPopUp.transform.SetParent(_rectTransform);

        uiPopUp.TextInit("돈이 부족합니다!", 30f, Color.red, _buyButton.transform.position);
        uiPopUp.UpAndFadeText();
    }

    private void CantButItem()
    {
        CantText();
        CantBuySound();
        _rectTransform.DOShakeAnchorPos(1f, 5f);
    }

    private void CantBuySound()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.clipData = _cantBuySoundSo;
        soundEvt.position= transform.position;
        _soundEventChannel.RaiseEvent(soundEvt);
    }
    
    private void CanBuySound()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.clipData = _buySoundSo;
        soundEvt.position= transform.position;
        _soundEventChannel.RaiseEvent(soundEvt);
    } 
}
