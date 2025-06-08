using IH.EventSystem.UIEvent;
using UnityEngine;
using YH.EventSystem;

public class SlotSelectImage : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    [SerializeField] private float _tweenTime;
    
    private void Awake()
    {
        _uiEventChannel.AddListener<ItemSlotSelectActiveEvent>(HandleItemSlotSelectActive);
        _uiEventChannel.AddListener<ItemSlotSelectEvent>(HandleItemSlotSelect);
        
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _uiEventChannel.RemoveListener<ItemSlotSelectActiveEvent>(HandleItemSlotSelectActive);        
        _uiEventChannel.RemoveListener<ItemSlotSelectEvent>(HandleItemSlotSelect);        
    }
    
    private void HandleItemSlotSelect(ItemSlotSelectEvent evt)
    {
        transform.SetParent(evt.targetTrm);
        transform.localPosition = Vector3.zero;
        (transform as RectTransform).sizeDelta = evt.targetTrm.sizeDelta;
        transform.localScale = Vector3.one * 0.8f;
    }

    private void HandleItemSlotSelectActive(ItemSlotSelectActiveEvent evt)
    {
        gameObject.SetActive(evt.isActive);
    }
}
