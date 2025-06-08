using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipPartInfo : MonoBehaviour
{
    [SerializeField] private Image _partImage;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    
    public void UpdateSlot(PartInventoryItem item)
    {
        _partImage.sprite = item.data.icon;
        _title.text = item.data.itemName;
        _description.text = item.data.itemDescription;
    }
}
