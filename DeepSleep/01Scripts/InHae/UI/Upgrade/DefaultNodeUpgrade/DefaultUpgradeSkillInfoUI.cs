using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using UnityEngine;
using YH.EventSystem;

public class DefaultUpgradeSkillInfoUI : UpgradeSkillInfouIBase
{
    [SerializeField] private GameEventChannelSO _defaultNodeEventChannelSO;

    protected override void Awake()
    {
        base.Awake();
        _defaultNodeEventChannelSO.AddListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);
        _defaultNodeEventChannelSO.AddListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);
        _defaultNodeEventChannelSO.AddListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _defaultNodeEventChannelSO.RemoveListener<UpgradeSkillSelectEvent>(HandleNodeUpgradeSkillInfo);        
        _defaultNodeEventChannelSO.RemoveListener<UpgradeSkillInitEvent>(HandleNodeUpgradeInit);        
        _defaultNodeEventChannelSO.RemoveListener<UpgradeSkillReLoadEvent>(HandleSkillReLoad);       
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

    protected override void SetUpInfo(SkillStatViewInitEvent evt)
    {
        base.SetUpInfo(evt);
        
        if(_selectedSkillItem == null)
            return;
        
        int currentCoin = _playerManagerSO.CurrentCoin;
        int upgradeCost = _selectedSkillItem.nodeGridDictionary.Count * 10;
        
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
}
