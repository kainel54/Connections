using DG.Tweening;
using IH.EventSystem.LevelEvent;
using IH.EventSystem.MissionEvent;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _missionEventChannelSO;
    [SerializeField] private GameEventChannelSO _levelEventChannelSO;
    
    [SerializeField] private TextMeshProUGUI _popUpText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _etcText;

    private void Awake()
    {
        _descriptionText.gameObject.SetActive(false);
        _popUpText.gameObject.SetActive(false);
        
        _missionEventChannelSO.AddListener<MissionInitEvent>(HandleMissionInitEvent);
        _missionEventChannelSO.AddListener<MissionCheckEvent>(HandleMissionCheckEvent);
        
        _missionEventChannelSO.AddListener<MissionEtcTextEvent>(HandleUsingEtcText);
        
        _levelEventChannelSO.AddListener<LevelMoveCompleteEvent>(HandleLevelMoveEvent);
    }

    private void OnDestroy()
    {
        _missionEventChannelSO.RemoveListener<MissionInitEvent>(HandleMissionInitEvent);
        _missionEventChannelSO.RemoveListener<MissionCheckEvent>(HandleMissionCheckEvent);
        
        _missionEventChannelSO.RemoveListener<MissionEtcTextEvent>(HandleUsingEtcText);
        
        _levelEventChannelSO.RemoveListener<LevelMoveCompleteEvent>(HandleLevelMoveEvent);
    }
    
    private void HandleUsingEtcText(MissionEtcTextEvent evt)
    {
        _etcText.DOFade(evt.isActive ? 1f : 0f, 0.5f);
        _etcText.color = evt.color;
        _etcText.text = evt.text;
    }
    
    private void HandleMissionCheckEvent(MissionCheckEvent evt)
    {
        if (evt.missionCheck)
            _descriptionText.DOColor(Color.green, 0.8f);
        else
            _descriptionText.DOColor(Color.red, 0.8f);
    }

    private void HandleMissionInitEvent(MissionInitEvent evt)
    {
        _descriptionText.text = evt.missionDescription;
        _popUpText.text = evt.missionDescription;
        
        _popUpText.gameObject.SetActive(true);
        _popUpText.color = Color.clear;
        _popUpText.DOColor(Color.white, 0.8f);
        
        DOVirtual.DelayedCall(3f,  ()=>_popUpText.DOColor(Color.clear, 0.8f))
            .OnComplete(()=>_descriptionText.gameObject.SetActive(true));
    }
    
    private void HandleLevelMoveEvent(LevelMoveCompleteEvent evt)
    {
        if (_descriptionText.gameObject.activeInHierarchy)
            _descriptionText.DOFade(0f, 0.6f);
    }
}
