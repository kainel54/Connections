using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using TMPro;
using UnityEngine;
using YH.EventSystem;

public class SpecialModeChangeText : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private GameEventChannelSO _specialPartNodeEventChannel;
    [SerializeField] private TextMeshProUGUI _currentViewModeText;
    
    private bool _isSpecialMode;

    private void Awake()
    {
        _skillNodeEventChannel.AddListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
        
        _specialPartNodeEventChannel.AddListener<SetSpecialModeEvent>(HandleCurrentViewModeChangeEvent);
        _specialPartNodeEventChannel.AddListener<ChangeSpecialModeEvent>(HandleSpecialModeChangeEvent);
    }
    

    private void OnDestroy()
    {
        _skillNodeEventChannel.RemoveListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
        
        _specialPartNodeEventChannel.RemoveListener<SetSpecialModeEvent>(HandleCurrentViewModeChangeEvent);
        _specialPartNodeEventChannel.RemoveListener<ChangeSpecialModeEvent>(HandleSpecialModeChangeEvent);
    }
    
    private void HandleInitNodeSkillEvent(InitNodeSkillEvent evt)
    {
        _isSpecialMode = false;
        _currentViewModeText.text = "현재 표시:파츠";
        _currentViewModeText.color = Color.white;
    }

    private void HandleSpecialModeChangeEvent(ChangeSpecialModeEvent evt)
    {
        _isSpecialMode = !_isSpecialMode;
        _currentViewModeText.text = _isSpecialMode ? "현재 표시:노드 능력" : "현재 표시:파츠";
        _currentViewModeText.color = _isSpecialMode ? Color.cyan : Color.white;
    }

    private void HandleCurrentViewModeChangeEvent(SetSpecialModeEvent evt)
    {
        _isSpecialMode = evt.isSpecialMode;
        _currentViewModeText.text = evt.isSpecialMode ? "현재 표시:노드 능력" : "현재 표시:파츠";
        _currentViewModeText.color = _isSpecialMode ? Color.cyan : Color.white;
    }
}
