using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillStatPopUpUI : MonoBehaviour
{
    [SerializeField] private Image _statImage;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    public void Init(SkillStatInfoSO skillStatInfoSo)
    {
        _statImage.sprite = skillStatInfoSo.icon;
        _titleText.SetText(skillStatInfoSo.title);
        _descriptionText.SetText(skillStatInfoSo.description);
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
