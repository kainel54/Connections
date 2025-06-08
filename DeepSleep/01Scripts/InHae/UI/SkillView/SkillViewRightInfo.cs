using DG.Tweening;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using UnityEngine;
using YH.EventSystem;

public class SkillViewRightInfo : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private float _tweenTime;
    
    [SerializeField] private RectTransform _equipPartView;
    [SerializeField] private RectTransform _partAndNodeItemView;
    
    private Vector3 _outPos;
    private Vector3 _inPos;

    private void Awake()
    {
        _outPos = _partAndNodeItemView.localPosition;
        _inPos = _equipPartView.localPosition;
        
        _skillNodeEventChannel.AddListener<EquipSkillSelectEvent>(HandlePartEquipSkillSelect);
    }

    private void OnDestroy()
    {
        _skillNodeEventChannel.RemoveListener<EquipSkillSelectEvent>(HandlePartEquipSkillSelect);
    }
    
    private void HandlePartEquipSkillSelect(EquipSkillSelectEvent evt)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);
        
        if (evt.isSelected)
        {
            sequence.Append(_partAndNodeItemView.DOLocalMove(_inPos, _tweenTime));
            sequence.Join(_equipPartView.DOLocalMove(_outPos, _tweenTime));
        }
        else
        {
            sequence.Append(_partAndNodeItemView.DOLocalMove(_outPos, _tweenTime));
            sequence.Join(_equipPartView.DOLocalMove(_inPos, _tweenTime));
        }
    }
}
