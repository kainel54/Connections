using DG.Tweening;
using TMPro;
using UnityEngine;
using YH.EventSystem;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    [SerializeField] private GameEventChannelSO _levelEventChannelSO;
    
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _popUpText;

    private void Awake()
    {
        _description.gameObject.SetActive(false);
        _popUpText.gameObject.SetActive(false);
        
        _uiEventChannelSO.AddListener<MissionInitEvent>(HandleMissionInitEvent);
        _uiEventChannelSO.AddListener<MissionCheckEvent>(HandleMissionCheckEvent);
        
        _levelEventChannelSO.AddListener<LevelMoveCompleteEvent>(HandleLevelMoveEvent);
    }

    private void OnDestroy()
    {
        _uiEventChannelSO.RemoveListener<MissionInitEvent>(HandleMissionInitEvent);
        _uiEventChannelSO.RemoveListener<MissionCheckEvent>(HandleMissionCheckEvent);
        
        _levelEventChannelSO.RemoveListener<LevelMoveCompleteEvent>(HandleLevelMoveEvent);
    }
    
    private void HandleMissionCheckEvent(MissionCheckEvent evt)
    {
        if (evt.missionCheck)
            _description.DOColor(Color.green, 0.8f);
        else
            _description.DOColor(Color.red, 0.8f);
    }

    private void HandleMissionInitEvent(MissionInitEvent evt)
    {
        _description.text = evt.missionDescription;
        _popUpText.text = evt.missionDescription;
        
        _popUpText.gameObject.SetActive(true);
        _popUpText.color = Color.clear;
        _popUpText.DOColor(Color.white, 0.8f);
        
        DOVirtual.DelayedCall(3f,  ()=>_popUpText.DOColor(Color.clear, 0.8f))
            .OnComplete(()=>_description.gameObject.SetActive(true));
    }
    
    private void HandleLevelMoveEvent(LevelMoveCompleteEvent evt)
    {
        if (_description.gameObject.activeInHierarchy)
            _description.DOFade(0f, 0.6f);
    }
}
