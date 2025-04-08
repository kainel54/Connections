using UnityEngine;

public class SkillUIWindow : WindowPanel
{
    [SerializeField] private UIInputReader _uiInputReader;
    [SerializeField] private Camera _skillUICamera;
    private CanvasGroup _canvasGroup;
    private Camera _mainCamera;
    private void Awake()
    {
        _mainCamera = Camera.main;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        
        _uiInputReader.TapEvent += HandleOpenSkillUI;
    }

    private void OnDestroy()
    {
        _uiInputReader.TapEvent -= HandleOpenSkillUI;
    }

    private void HandleOpenSkillUI()
    {
        var evt = UIEvents.WindowPanelOpenEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);
    }

    public override void OpenWindow()
    {
        _mainCamera.gameObject.SetActive(false);
        _skillUICamera.gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public override void CloseWindow()
    {
        _mainCamera.gameObject.SetActive(true);
        _skillUICamera.gameObject.SetActive(false);
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
}
