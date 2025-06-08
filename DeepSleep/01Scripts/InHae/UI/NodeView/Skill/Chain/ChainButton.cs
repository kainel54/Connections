using IH.EventSystem.NodeEvent.NodeChainEvent;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class ChainButton : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _nodeChainEventChannel;
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private TextMeshProUGUI _text;
    
    private Button _button;

    private bool _isChainMode = true;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ChainButtonClicked);
        
        _skillNodeEventChannel.AddListener<InitNodeSkillEvent>(HandleChainModeInit);
        _nodeChainEventChannel.AddListener<ChainModeChangeEvent>(HandleChainModeChange);
    }
    
    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ChainButtonClicked);
        _skillNodeEventChannel.RemoveListener<InitNodeSkillEvent>(HandleChainModeInit);
        _nodeChainEventChannel.RemoveListener<ChainModeChangeEvent>(HandleChainModeChange);
    }
    
    private void HandleChainModeInit(InitNodeSkillEvent evt)
    {
        ChangeChainMode(false);
        _isChainMode = true;
    }
    
    private void ChainButtonClicked()
    {
        ChangeChainMode(_isChainMode);
        _isChainMode = !_isChainMode;
    }

    private void ChangeChainMode(bool isActive)
    {
        var evt = NodeChainEvents.ChainModeChangeEvent;
        evt.isActive = isActive;
        _nodeChainEventChannel.RaiseEvent(evt);
    }
    
    private void HandleChainModeChange(ChainModeChangeEvent evt)
    {
        _text.text = evt.isActive ? "취소" : "체인 모드";
    }

    public void VariableInit()
    {
        _isChainMode = false;
        ChangeChainMode(_isChainMode);
        _isChainMode = true;
    }
}
