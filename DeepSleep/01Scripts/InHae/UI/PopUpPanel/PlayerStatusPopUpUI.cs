using TMPro;
using UnityEngine;

public class PlayerStatusPopUpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    public void Init(StatusDataSO statusDataSo)
    {
        _titleText.SetText(statusDataSo.statusName);
        _descriptionText.SetText(statusDataSo.description);
    }

    public void OnPopUp()
    {
        gameObject.SetActive(true);
    }

    public void EndPopUp()
    {
        gameObject.SetActive(false);
    }
}
