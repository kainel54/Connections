using UnityEngine;
using UnityEngine.EventSystems;

public class PartNodeUIPointerAction : MonoBehaviour, IPartNodeUIComponent, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, 
    IPointerEnterHandler, IPointerExitHandler
{
    protected PartNodeUI _partNodeUI;
    
    protected RectTransform _dragTarget;
    protected DragItem _dragItem;
    
    protected PartNodeUIOnPopUp _onPopUpCompo;
    
    protected NodeEquipData _currentEquipData => _partNodeUI.CurrentEquipData;

    private void Awake()
    {
        _dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
    }

    public virtual void Initialize(PartNodeUI partNodeUI)
    {
        _partNodeUI = partNodeUI;
        _onPopUpCompo = _partNodeUI.GetCompo<PartNodeUIOnPopUp>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || _partNodeUI.isPartEmpty)
            return;

        _dragItem.StartDrag(_currentEquipData.partInventoryItem);
        _dragTarget = _dragItem.rectTransform;
        _dragTarget.position = Input.mousePosition;

        _partNodeUI.UpdateSlotImage(null, Color.clear);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        var dragItem = UIHelper.Instance.GetDragItem(DragItemType.NodeInPart);
        dragItem.EndDrag();

        if (_partNodeUI.isPartEmpty || dragItem.successDrop)
            return;

        Color color = Color.white;
        color.a = _partNodeUI.isSpecialMode ? 0.3f : 1f;
        _partNodeUI.UpdateSlotImage(_currentEquipData.partInventoryItem.data.icon, color);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || _partNodeUI.isPartEmpty)
            return;

        _dragTarget.position = Input.mousePosition;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Right || _partNodeUI.isSpecialMode)
            return;
        
        _onPopUpCompo.EndPopUp();
        _partNodeUI.ReturnInventoryItem();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        
        if(_partNodeUI.isPartEmpty)
            return;

        var PartPopUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
        _onPopUpCompo.SetPopUpOn(PartPopUp, _currentEquipData.partInventoryItem);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(_partNodeUI.isPartEmpty)
            return;
        _onPopUpCompo.EndPopUp();
    }
}
