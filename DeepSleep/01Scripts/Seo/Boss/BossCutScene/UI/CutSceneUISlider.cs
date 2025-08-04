using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneUISlider : MonoBehaviour
{
    [SerializeField] private Image _upImage;
    [SerializeField] private Image _downImage;
    public void StartSlide()
    {
        Debug.Log("¿Ã∞≈µµ µ ");

        _upImage.rectTransform.DOAnchorPos(Vector3.zero, 0.5f).SetUpdate(true);
        _downImage.rectTransform.DOAnchorPos(Vector3.zero * 10, 0.5f).SetUpdate(true);
    }
}
