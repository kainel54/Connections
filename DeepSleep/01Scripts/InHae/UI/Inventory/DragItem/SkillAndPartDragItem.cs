
using System;
using TMPro;
using UnityEngine;

public class SkillAndPartDragItem : DragItem
{
    [SerializeField] private GameObject _frame;
    [SerializeField] private TextMeshProUGUI _amountText;

    public override void StartDrag(InventoryItem item)
    {
        base.StartDrag(item);
        gameObject.SetActive(true);
        _inventoryItem = item;
        _itemImage.sprite = item.data.icon;

        _itemImage.color = Color.white;
        _amountText.text = item.stackSize > 1 ? item.stackSize.ToString() : string.Empty;

        successDrop = false;

        _frame.SetActive(true);
    }

    public override void EndDrag()
    {
        base.EndDrag();
        gameObject.SetActive(false);
        _itemImage.color = Color.clear;
        _amountText.text = String.Empty;
        
        _frame.SetActive(false);
    }
}
