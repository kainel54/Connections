using IH.EventSystem.UIEvent.PanelEvent;
using ObjectPooling;
using TMPro;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    [SerializeField] private PlayerManagerSO _playerManager;
    [SerializeField] private TextMeshPro _priceText;

    private DropItem _item;
    private ItemDataSO _itemData;
    private Transform _itemTrm;

    private Collider _playerCollider;
    private PoolingNoLifeTimeEffectPlayer _glowEffect;

    public void Init(DropItem item)
    {
        _itemTrm = transform.Find("ItemTrm");
        
        _item = PoolManager.Instance.Pop(item.PoolEnum) as DropItem;
        if (_item == null || _item.itemData == null)
        {
            Debug.LogError("Item Is Null");
            return;
        }
        
        _itemData = _item.itemData;
        _item.transform.SetParent(_itemTrm);
        _item.transform.position = _itemTrm.position;
        _item.SphereCollider.enabled = false;

        _priceText.text = _item.itemData.price + "$";   
        
        if (_item is ISpecialInitItem specialInitItem)
            specialInitItem.VisualInit();

        PlayGlowEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var evt = UIPanelEvent.ShopDescriptionPanelEvent;
            evt.isPanelActive = true;
            evt.itemDataSo = _item.itemData;
            evt.canBuyItem = _playerManager.CurrentCoin >= _item.itemData.price;
            
            if(_item.itemData as NodeAbilityItemSO)
                evt.textColor = Color.cyan;
            else
                evt.textColor = Color.white;
            
            evt.buyItemAction += Sold;
            _playerCollider = other;

            _uiEventChannel.RaiseEvent(evt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        UiDisable();
    }

    private void Sold()
    {
        _item.PickUp(_playerCollider);

        _playerManager.AddCoin(-_item.itemData.price);
        gameObject.SetActive(false);

        if (_glowEffect != null)
            PoolManager.Instance.Push(_glowEffect);
        
        UiDisable();
    }

    private void UiDisable()
    {
        var evt = UIPanelEvent.ShopDescriptionPanelEvent;
        evt.isPanelActive = false;
        evt.itemDataSo = null;
        evt.buyItemAction -= Sold;

        _uiEventChannel.RaiseEvent(evt);
    }
    
    private void PlayGlowEffect()
    {
        if(_itemData ==null || _itemData.itemTier == ItemTier.Normal)
            return;

        switch (_itemData.itemTier)
        {
            case ItemTier.Rare:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemRareGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
            case ItemTier.Epic:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemEpicGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
            case ItemTier.Legendary:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemLegendaryGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
        }

        _glowEffect.PlayEffect(_itemTrm.position, Quaternion.identity, Vector3.one, transform);
    }
}
