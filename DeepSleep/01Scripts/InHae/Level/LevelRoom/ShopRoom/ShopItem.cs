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
    private Transform _itemTrm;

    private Collider _playerCollider;

    public void Init(DropItem item)
    {
        _itemTrm = transform.Find("ItemTrm");
        
        _item = Instantiate(item, _itemTrm);
        _item.transform.position = _itemTrm.position;
        _item.SphereCollider.enabled = false;
        
        _priceText.text = _item.itemData.price + "$";   
        
        if (_item is PartsObject partsObject)
            partsObject.VisualInit();

        if (_item is SkillObject skillObject)
            skillObject.VisualInit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var evt = UIEvents.ShopDescriptionEvent;
            evt.isPanelActive = true;
            evt.itemDataSo = _item.itemData;
            evt.canBuyItem = _playerManager.CurrentCoin >= _item.itemData.price;
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
        UiDisable();
    }

    private void UiDisable()
    {
        var evt = UIEvents.ShopDescriptionEvent;
        evt.isPanelActive = false;
        evt.itemDataSo = null;
        evt.buyItemAction -= Sold;

        _uiEventChannel.RaiseEvent(evt);
    }
}
