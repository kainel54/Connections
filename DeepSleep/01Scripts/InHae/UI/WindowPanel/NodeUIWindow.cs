using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class NodeUIWindow : WindowPanel
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannelSO;
    [SerializeField] private float _tweenTime;
    
    private List<Scrollbar> _scrollbars;

    private void Awake()
    {
        _scrollbars = GetComponentsInChildren<Scrollbar>().ToList();
    }

    public override void OpenWindow()
    {
        foreach (var scrollbar in _scrollbars)
            scrollbar.value = 1f;

        transform.DOScale(Vector3.one, _tweenTime).SetUpdate(true);

        var skillSelectEvt = SkillNodeEvents.EquipSkillSelectEvent;
        skillSelectEvt.isSelected = true;
        _skillNodeEventChannelSO.RaiseEvent(skillSelectEvt);
    }

    public override void CloseWindow()
    {
        var panel = UIHelper.Instance.GetPopUpPanel(ItemPopUpItemType.Chain);
        panel.EndPopUp();

        transform.DOScale(Vector3.zero, _tweenTime).SetUpdate(true);
        
        var skillSelectEvt = SkillNodeEvents.EquipSkillSelectEvent;
        skillSelectEvt.isSelected = false;
        _skillNodeEventChannelSO.RaiseEvent(skillSelectEvt);
    }
}
