using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum ItemPopUpItemType
{
    Skill,
    Part,
    Chain,
}

public abstract class ItemPopUpPanel : MonoBehaviour
{
    public ItemPopUpItemType popUpType;
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemTitle;
    [SerializeField] protected TextMeshProUGUI _itemDescription;
    
    [HideInInspector] public RectTransform rectTransform;
    protected InventoryItem _inventoryItem;

    protected bool _isFix;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void OnPopUp(InventoryItem item)
    {
        _isFix = false;
    }

    public void SetFix(bool isFix)
    {
        _isFix = isFix;
    }

    public abstract void EndPopUp();

    public InventoryItem GetDraggedItem() => _inventoryItem;
}
