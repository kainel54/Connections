using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class SkillInfoPanelUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    private SkillResultDescription _skillResultDescription;
    
    [SerializeField] private Image _skillIcon;
    private TextMeshProUGUI _skillName;
    private TextMeshProUGUI _skillAttackTypeAndTier;

    private Dictionary<SkillFieldDataType, SkillStatTextGroupUI> _statTextUI = new ();
    
    [SerializeField] private float _tweenTime;
    [SerializeField] private float _offset;
    private RectTransform _arrowTrm;
    private Vector3 _arrowDefaultScale;
    
    private bool _isOpen = true;
    private Vector3 _initPos;

    private void Awake()
    {
        _skillResultDescription = GetComponentInChildren<SkillResultDescription>();
        
        _skillName = transform.Find("TopGroup/Texts/SkillName").GetComponent<TextMeshProUGUI>();
        _skillAttackTypeAndTier = transform.Find("TopGroup/Texts/AttackTypeAndTierText").GetComponent<TextMeshProUGUI>();
        
        _arrowTrm = transform.Find("InOutButton/Arrow").transform as RectTransform;
        
        _initPos = transform.localPosition;
        _arrowDefaultScale = _arrowTrm.localScale;
            
        GetComponentsInChildren<SkillStatTextGroupUI>().ToList()
            .ForEach(x => _statTextUI.Add(x.fieldType, x));
        
        _skillNodeEventChannel.AddListener<SkillStatViewInitEvent>(HandleSkillStatViewInit);
        _skillNodeEventChannel.AddListener<EquipSkillSelectEvent>(HandleSkillSelect);
    }

    private void OnDestroy()
    {
        _skillNodeEventChannel.RemoveListener<SkillStatViewInitEvent>(HandleSkillStatViewInit);
        _skillNodeEventChannel.RemoveListener<EquipSkillSelectEvent>(HandleSkillSelect);
    }
    
    private void HandleSkillSelect(EquipSkillSelectEvent evt)
    {
        if (evt.isSelected)
        {
            Vector3 pos = _initPos;
            pos.x = 0;
            pos.x += _offset;

            transform.DOLocalMove(pos, _tweenTime).SetUpdate(true);
        }
        else
        {
            transform.DOLocalMove(_initPos, _tweenTime).SetUpdate(true);
            _arrowTrm.localScale = _arrowDefaultScale;
            _isOpen = true;
        }
    }

    private void HandleSkillStatViewInit(SkillStatViewInitEvent evt)
    {
        SkillItemSO skillItemSo = evt.skillInventoryItem.data as SkillItemSO;
        
        _skillIcon.sprite = skillItemSo.icon;
        
        _skillName.text = skillItemSo.itemName;
        float fontSize = Mathf.Clamp(_skillName.rectTransform.sizeDelta.x / _skillName.text.Length, 1f, 60f);
        _skillName.fontSize = fontSize;

        GenericSkillDataSO genericSkillData = evt.skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;
        string attackType = genericSkillData.attackType == SkillAttackType.Melee ? "근접" : "원거리";
        _skillAttackTypeAndTier.text = attackType + " " + ItemTierStringParser.TierToString(skillItemSo.itemTier);

        _skillResultDescription.ResultDescription(skillItemSo, evt.skill);
       
        foreach (var statTextUI in _statTextUI)
            statTextUI.Value.Init(evt.skill.GetSkillData(statTextUI.Key), evt.skill);
    }
    
    // Button 함수
    public void ButtonClickInOut()
    {
        Vector3 scale = _arrowTrm.localScale;
        scale.x *= -1;
        _arrowTrm.localScale = scale;

        Vector3 pos = transform.localPosition;
        
        if (_isOpen)
            pos.x -= _offset;
        else
            pos.x += _offset;

        transform.DOLocalMove(pos, _tweenTime).SetUpdate(true);
        _isOpen = !_isOpen;
    }
}
