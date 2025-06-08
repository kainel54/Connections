using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;

public class DefaultUpgradeSkillInfoUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannelSO;
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private WindowPanel _upgradeWindow;
    [SerializeField] private Image _skillImage;

    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private TextMeshProUGUI _priceText;
    
    private Sprite _defaultSprite;

    private bool _isUpgradeAble;
    private SkillInventoryItem _selectedSkill;
    
    private void Awake()
    {
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        _priceText = transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
        
        _defaultNodeEventChannelSO.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
        _defaultNodeEventChannelSO.AddListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);
        _defaultNodeEventChannelSO.AddListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);
        
        _defaultSprite = _skillImage.sprite;
    }
    
    private void OnDestroy()
    {
        _defaultNodeEventChannelSO.RemoveListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);        
        _defaultNodeEventChannelSO.RemoveListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);        
        _defaultNodeEventChannelSO.RemoveListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);        
    }
    
    private void HandleSkillReLoad(UpgradeSkillReLoadEvent evt) => SetUpInfo();

    private void HandleNodeUpgradeSkillInfo(UpgradeSkillSelectEvent evt)
    {
        _selectedSkill = evt.item;
        SetUpInfo();
    }
    
    private void HandleNodeUpgradeInit(UpgradeSkillInitEvent evt)
    {
        _title.SetText("");
        _description.SetText("");
        _priceText.SetText("");
        
        _isUpgradeAble = false;
        _skillImage.sprite = _defaultSprite;
    }

    private void SetUpInfo()
    {
        SkillItemSO skillItemSO = _selectedSkill.data as SkillItemSO;
        _title.SetText(skillItemSO.itemName);
        _description.SetText(skillItemSO.itemDescription);
        _skillImage.sprite = skillItemSO.icon;
        
        int currentCoin = _playerManagerSO.CurrentCoin;
        int upgradeCost = _selectedSkill.nodeGridDictionary.Count * 10;
        
        if (currentCoin >= upgradeCost)
        {
            _isUpgradeAble = true;
            _priceText.color = Color.green;
        }
        else
        {
            _isUpgradeAble = false;
            _priceText.color = Color.red;
        }
        
        _priceText.text = $"{currentCoin} / {upgradeCost}";
    }

    public void OpenUpgradeWindow()
    {
        if (!_isUpgradeAble)
            return;
        
        var evt = UIPanelEvent.WindowPanelToggleEvent;
        evt.currentWindow = _upgradeWindow;
        _uiEventChannelSO.RaiseEvent(evt);
    }
}
