using IH.EventSystem.LevelEvent;
using IH.EventSystem.MissionEvent;
using System;
using TMPro;
using UnityEngine;
using YH.EventSystem;
using DG.Tweening;
public class TutorialUI : MonoBehaviour
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

        _missionEventChannelSO.AddListener<TutorialMissionInit>(HandleMissionInitEvent);
        _missionEventChannelSO.AddListener<TutorialMissionCheckEvent>(HandleMissionCheckEvent);

        _missionEventChannelSO.AddListener<TutorialMissionEtcTextEvent>(HandleUsingEtcText);

        _levelEventChannelSO.AddListener<LevelMoveCompleteEvent>(HandleLevelMoveEvent);
    }

    private void HandleLevelMoveEvent(LevelMoveCompleteEvent evt)
    {
        if (_descriptionText.gameObject.activeInHierarchy)
            _descriptionText.DOFade(0f, 0.6f);
    }

    private void HandleUsingEtcText(TutorialMissionEtcTextEvent evt)
    {
        _etcText.DOFade(evt.isActive ? 1f : 0f, 0.5f);
        _etcText.color = evt.color;
        _etcText.text = evt.text;
    }

    private void HandleMissionCheckEvent(TutorialMissionCheckEvent evt)
    {
        if (evt.missionCheck)
            _descriptionText.DOColor(Color.green, 0.8f);
        else
            _descriptionText.DOColor(Color.red, 0.8f);
    }

    private void HandleMissionInitEvent(TutorialMissionInit evt)
    {
        _descriptionText.text = evt.tutorialDescription;
        _popUpText.text = evt.tutorialDescription;

        _popUpText.gameObject.SetActive(true);
        _popUpText.color = Color.clear;
        _popUpText.DOColor(Color.white, 0.8f);

        DOVirtual.DelayedCall(3f, () => _popUpText.DOColor(Color.clear, 0.8f))
            .OnComplete(() => _descriptionText.gameObject.SetActive(true));
    }
}
