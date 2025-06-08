using System.Linq;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;
using YH.Players;

public class SpecialUpgradeSkillInfoUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _specialNodeEventChannelSO;
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private WindowPanel _upgradeWindow;
    [SerializeField] private Image _skillImage;

    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private TextMeshProUGUI _priceText;
    private TextMeshProUGUI _upgradeAbleCountText;
    
    private Sprite _defaultSprite;

    private bool _isUpgradeAble;
    private SkillInventoryItem _selectedSkill;
    
    private void Awake()
    {
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        _priceText = transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
        _upgradeAbleCountText = transform.Find("UpgradeAbleCount").GetComponent<TextMeshProUGUI>();
        
        _specialNodeEventChannelSO.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
        _specialNodeEventChannelSO.AddListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);
        _specialNodeEventChannelSO.AddListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);
        
        _defaultSprite = _skillImage.sprite;
    }
    
    private void OnDestroy()
    {
        _specialNodeEventChannelSO.RemoveListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);        
        _specialNodeEventChannelSO.RemoveListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);        
        _specialNodeEventChannelSO.RemoveListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);        
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
        _upgradeAbleCountText.SetText("");
        
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
        int specialNodeCount = _selectedSkill.nodeGridDictionary.Values.Count(x => x.isSpecial);

        int upgradeAbleNodeCount = _selectedSkill.nodeGridDictionary.Count - specialNodeCount;
        _upgradeAbleCountText.SetText($"업그레이드 가능한 노드 수: {upgradeAbleNodeCount}");
        
        int upgradeCost = specialNodeCount * 50;
        if (specialNodeCount == 0)
            upgradeCost = 25;
        
        if (currentCoin < upgradeCost || upgradeAbleNodeCount == 0)
            _isUpgradeAble = false;
        else
            _isUpgradeAble = true;
        
        _upgradeAbleCountText.color = upgradeAbleNodeCount == 0 ? Color.red : Color.green;
        _priceText.color = currentCoin < upgradeCost ? Color.red : Color.green;
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
