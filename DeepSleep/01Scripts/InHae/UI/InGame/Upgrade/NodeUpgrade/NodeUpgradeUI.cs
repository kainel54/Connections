using IH.Manager;
using UnityEngine;

public class NodeUpgradeUI : WindowPanel
{
    [SerializeField] private Camera _skillUICamera;
    [SerializeField] private RectTransform _parent;
    
    private CanvasGroup _canvasGroup;
    private Camera _mainCamera;
    
    private SkillStash _stash;
    private SkillInventory _inventory;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;

    }

    private void Start()
    {
        _inventory = InventoryManager.Instance.GetInventory(InventoryType.Skill) as SkillInventory;
    }

    public void HandleOpenUI()
    {
        var evt = UIEvents.WindowPanelOpenEvent;
        evt.currentWindow = this;
        _uiEventChannel.RaiseEvent(evt);

        _stash = new SkillStash(_parent, _inventory.GetStash());
        _stash.UpdateSlotUI();
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
        
        _inventory.SetStash(_stash);
    }
}
