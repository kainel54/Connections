using UnityEngine;
using UnityEngine.Events;

public class NodeUIWindow : WindowPanel
{
    public UnityEvent closeEvent;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();        
    }

    public override void OpenWindow()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public override void CloseWindow()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        
        closeEvent?.Invoke();
        var panel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Chain);
        panel.EndPopUp();
    }
}
