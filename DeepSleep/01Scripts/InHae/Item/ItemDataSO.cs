using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "SO/Item/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public InventoryType inventoryType;
    
    public Sprite icon;
    public string itemName;
    public string itemDescription;
    
    public int price;
    public int maxStack = 999;
}
