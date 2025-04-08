using System;
using DG.Tweening;
using ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class ShopPanelUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button _buyButton;
    
    private RectTransform _rectTransform => transform as RectTransform;
    
    private void Awake()
    {
        _uiEventChannel.AddListener<ShopDescriptionEvent>(HandleShopDescription);
    }

    private void OnDestroy()
    {
        _uiEventChannel.RemoveListener<ShopDescriptionEvent>(HandleShopDescription);
    }

    private void HandleShopDescription(ShopDescriptionEvent evt)
    {
        if (evt.isPanelActive)
        {
            _titleText.text = evt.itemDataSo.itemName;
            _descriptionText.text = evt.itemDataSo.itemDescription;
            _priceText.text = evt.itemDataSo.price + "$";  
            _iconImage.sprite = evt.itemDataSo.icon;

            _priceText.color = evt.canBuyItem ? Color.yellow : Color.red;

            if (evt.canBuyItem)
            {
                _buyButton.onClick.AddListener(evt.buyItemAction.Invoke);
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
        UIPopUpText uiPopUp = PoolManager.Instance.Pop(PoolingType.UIPopUpText) as UIPopUpText;
        uiPopUp.transform.SetParent(_rectTransform);

        uiPopUp.TextInit("돈이 부족합니다!", 30f, Color.red, _buyButton.transform.position);
        uiPopUp.UpAndFadeText();
    }

    private void CantButItem()
    {
        CantText();
        _rectTransform.DOShakeAnchorPos(1f, 5f);
    }
}
