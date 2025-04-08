using UnityEngine;
using UnityEngine.EventSystems;

public class SkillItemSlotUI : ItemSlotUI, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float _yOffset;
    [SerializeField] private GameObject _skillImageObj;
    [SerializeField] private Canvas _canvas;
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
        
        _skillImageObj.SetActive(false);
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Skill);
        popUp.SetFix(false);
        popUp.EndPopUp();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        
        var drag = UIHelper.Instance.GetDragItem(DragItemType.SkillAndPart);
        if(drag.isDragging)
            return;
  
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Skill);
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
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Skill);
        popUp.EndPopUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isEmpty)
            return;
        var popUp = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Skill);
        popUp.SetFix(true);
    }
}
