using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum ItemPopUpItemType
{
    Part,
    Chain,
    NodeAbility,
    SkillStat,
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

    public bool isOnPopUp;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void OnPopUp(InventoryItem item)
    {
        _isFix = false;
        isOnPopUp = true;
    }

    public void SetFix(bool isFix)
    {
        _isFix = isFix;
    }

    public virtual void EndPopUp()
    {
        isOnPopUp = false;
    }

    public InventoryItem GetDraggedItem() => _inventoryItem;
}
