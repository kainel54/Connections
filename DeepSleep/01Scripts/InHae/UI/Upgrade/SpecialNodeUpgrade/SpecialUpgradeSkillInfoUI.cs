using System.Linq;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using TMPro;
using UnityEngine;
using YH.EventSystem;

public class SpecialUpgradeSkillInfoUI : UpgradeSkillInfouIBase
{
    [SerializeField] private GameEventChannelSO _specialNodeEventChannelSO;
    private TextMeshProUGUI _upgradeAbleCountText;

    protected override void Awake()
    {
        base.Awake();
        _upgradeAbleCountText = transform.Find("UpgradeAbleCount").GetComponent<TextMeshProUGUI>();
        
        _specialNodeEventChannelSO.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
        _specialNodeEventChannelSO.AddListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);
        _specialNodeEventChannelSO.AddListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _specialNodeEventChannelSO.RemoveListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);        
        _specialNodeEventChannelSO.RemoveListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);        
        _specialNodeEventChannelSO.RemoveListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);        
    }
    
    private void HandleSkillReLoad(UpgradeSkillReLoadEvent evt) => SetUpInfo(null);

    private void HandleNodeUpgradeSkillInfo(UpgradeSkillSelectEvent evt)
    {
        _selectedSkillItem = evt.item;

        SetDescription();
    }
    
    private void HandleNodeUpgradeInit(UpgradeSkillInitEvent evt)
    {
        InfoInit();
    }

    protected override void InfoInit()
    {
        base.InfoInit();
        _upgradeAbleCountText.SetText("");
    }

    protected override void SetUpInfo(SkillStatViewInitEvent evt)
    {
        base.SetUpInfo(evt);
        if(_selectedSkillItem == null)
            return;
        
        int currentCoin = _playerManagerSO.CurrentCoin;
        int specialNodeCount = _selectedSkillItem.nodeGridDictionary.Values.Count(x => x.isSpecial);

        int upgradeAbleNodeCount = _selectedSkillItem.nodeGridDictionary.Count - specialNodeCount;
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
}
