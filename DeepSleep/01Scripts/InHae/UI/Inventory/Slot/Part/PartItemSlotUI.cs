using IH.EventSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using YH.EventSystem;

public class PartItemSlotUI : ItemSlotUI, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameEventChannelSO _nodeEventChannel;
    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _skillImageObj;
    [SerializeField] private float _yOffset;

    private bool _isChainMode;

    private void Awake()
    {
        _nodeEventChannel.AddListener<ChainModeChangeEvent>(HandleChainModeChangeEvent);
    }

    private void OnDestroy()
    {
        _nodeEventChannel.RemoveListener<ChainModeChangeEvent>(HandleChainModeChangeEvent);
    }

    private void HandleChainModeChangeEvent(ChainModeChangeEvent evt)
    {
        _isChainMode = evt.isActive;
    }

    public override void UpdateSlot(InventoryItem newItem)
    {
        _skillImageObj.SetActive(true);
        item = newItem;
        _itemImage.color = Color.white;
        
        if(!isEmpty)
        {
            _itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
                _amountText.text = item.stackSize.ToString(); 
            else
                _amountText.text = string.Empty;
            
            item.slotIndex = slotIndex;
        }
        else
        {
            CleanUpSlot();
        }
    }

    public override void CleanUpSlot()
    {
        base.CleanUpSlot();
        _skillImageObj.SetActive(false);
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if(eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
        popUp.SetFix(false);
        popUp.EndPopUp();
        _skillImageObj.SetActive(false);

        ChainAbleEvent();
    }

    private void ChainAbleEvent()
    {
        if(!_isChainMode)
            return;

        var evt = NodeEvents.ChainPartSelectEvent;
        evt.partItemSO = item.data as PartItemSO;
        _nodeEventChannel.RaiseEvent(evt);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        
        var drag = UIHelper.Instance.GetDragItem(DragItemType.SkillAndPart);
        if(drag.isDragging)
            return;
  
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
        Vector3 pos = eventData.position;
        pos.y += _yOffset;

        RectTransform popUpRect = popUp.transform as RectTransform;
        RectTransform canvasRect = _canvas.transform as RectTransform;

        Vector2 popUpSize = popUpRect.sizeDelta;
        Vector2 screenSize = canvasRect.sizeDelta;

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, 
            pos, _canvas.worldCamera, out anchoredPos);

        float halfWidth = popUpSize.x * 0.5f;

        if (anchoredPos.x + halfWidth > screenSize.x * 0.5f)
            anchoredPos.x = screenSize.x * 0.5f - halfWidth;
        else if (anchoredPos.x - halfWidth < -screenSize.x * 0.5f)
            anchoredPos.x = -screenSize.x * 0.5f + halfWidth;

        popUpRect.anchoredPosition = anchoredPos;
        popUp.OnPopUp(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
        popUp.EndPopUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
        popUp.SetFix(true);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        var evt = NodeEvents.ChainPartSelectCompleteEvent;
        _nodeEventChannel.RaiseEvent(evt);
    }
}
