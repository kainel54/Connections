using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleSoundSetting : MonoBehaviour
{
    [SerializeField] private Image _backGroundImage;
    private bool _isOpen;

    private void Update()
    {
        if(_isOpen && Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open()
    {
        _isOpen = true;
        _backGroundImage.raycastTarget = true;
        _backGroundImage.DOFade(0.7f, 0.5f).SetUpdate(true);
        transform.DOScale(Vector3.one, 0.5f).SetUpdate(true);
    }
    
    public void Close()
    {
        _isOpen = false;
        _backGroundImage.DOFade(0, 0.5f).SetUpdate(true).OnComplete(() 
            => _backGroundImage.raycastTarget = false);
        transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true);
    }
}
