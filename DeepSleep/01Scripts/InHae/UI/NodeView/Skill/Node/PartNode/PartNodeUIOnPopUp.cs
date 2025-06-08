using UnityEngine;

public class PartNodeUIOnPopUp : MonoBehaviour, IPartNodeUIComponent
{
    [SerializeField] protected float _xOffset;
    protected ItemPopUpPanel _popUpPanel;

    private void Awake()
    {
        _popUpPanel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Part);
    }

    public virtual void Initialize(PartNodeUI partNodeUI)
    {
        
    }
    
    public void SetPopUpOn(ItemPopUpPanel popUpPanel, InventoryItem item)
    {
        if (item == null)
            return;
        
        RectTransform popUpRect = popUpPanel.transform as RectTransform;
        Vector2 popUpSize = popUpRect.sizeDelta;
        
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x -= popUpSize.x * 0.5f + _xOffset;
        popUpRect.position = pos;
        
        popUpPanel.OnPopUp(item);
    }
    
    public void CurrentPopUpOn(InventoryItem item)
    {
        if (item == null)
            return;
        
        RectTransform popUpRect = _popUpPanel.transform as RectTransform;
        Vector2 popUpSize = popUpRect.sizeDelta;
        
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos.x -= popUpSize.x * 0.5f + _xOffset;
        popUpRect.position = pos;
        
        _popUpPanel.OnPopUp(item);
    }

    public void EndPopUp()
    {
        _popUpPanel.EndPopUp();
    }
}
