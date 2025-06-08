using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DragItemType
{
    InventorySlotItem,
    NodeInPart,
}

public abstract class DragItem : MonoBehaviour
{
    public DragItemType dragItemType;
    
    [SerializeField] protected Image _itemImage;
    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public bool successDrop = false;
    
    public bool isDragging = false;

    protected InventoryItem _inventoryItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void StartDrag(InventoryItem item)
    {
        isDragging = true;
    }

    public virtual void EndDrag()
    {
        isDragging = false;
    }

    public InventoryItem GetDraggedItem() => _inventoryItem;
}
