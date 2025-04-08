using DG.Tweening;
using UnityEngine;

public class PauseWindow : WindowPanel
{
    [SerializeField] private float _offset;
    [SerializeField] private RectTransform _uiPanel;

    private Vector3 _settingOriginPosition;
    
    private void Awake()
    {
        _settingOriginPosition = _uiPanel.position;
    }

    public override void OpenWindow()
    {
        _uiPanel.DOLocalMoveY(_settingOriginPosition.y, 0.2f);
    }

    public override void CloseWindow()
    {
        _uiPanel.DOLocalMoveY(_settingOriginPosition.y + _offset, 0.5f);
    }
}
