using UnityEngine;
using UnityEngine.UI;

public class SkillPopUpPartSlot : MonoBehaviour
{
    [SerializeField] private GameObject _skillImageObj;
    [SerializeField] private Image _image;

    public void UpdateSlot(PartInventoryItem item)
    {
        _skillImageObj.SetActive(true);
        _image.sprite = item.data.icon;
    }

    public void CleanUpSlot()
    {
        _skillImageObj.SetActive(false);
        _image.sprite = null;
    }
}
