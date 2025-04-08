using TMPro;
using UnityEngine;

public class ChainButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void ChangeText(bool isActive)
    {
        _text.text = isActive ? "취소" : "체인 모드";
    }
}
