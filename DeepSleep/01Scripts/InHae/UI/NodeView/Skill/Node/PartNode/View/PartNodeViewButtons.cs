using IH.EventSystem.NodeEvent.NodeChainEvent;
using IH.EventSystem.NodeEvent.SkillNodeEvents;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using YH.EventSystem;

public class PartNodeViewButtons : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private GameEventChannelSO _nodeChainEventChannel;
    [SerializeField] private GameEventChannelSO _specialNodeEventChannel;
    
    [SerializeField] private float _offset;
    
    [SerializeField] private GameObject _partItemView;
    [SerializeField] private GameObject _nodeItemView;

    private bool _isOpen = true;
    private bool _isSpecialMode = false;

    private void Awake()
    {
        _skillNodeEventChannel.AddListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
        
        _specialNodeEventChannel.AddListener<ChangeSpecialModeEvent>(HandleChangeSpecialModeEvent);
        _specialNodeEventChannel.AddListener<SetSpecialModeEvent>(HandleSetSpecialModeEvent);
    }

    private void OnDestroy()
    {
        _skillNodeEventChannel.RemoveListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
        
        _specialNodeEventChannel.RemoveListener<ChangeSpecialModeEvent>(HandleChangeSpecialModeEvent);
        _specialNodeEventChannel.RemoveListener<SetSpecialModeEvent>(HandleSetSpecialModeEvent);
    }

    private void HandleSetSpecialModeEvent(SetSpecialModeEvent evt) => _isSpecialMode = evt.isSpecialMode;
    
    private void HandleChangeSpecialModeEvent(ChangeSpecialModeEvent evt)
    {
        _isSpecialMode = !_isSpecialMode;
        if (_isSpecialMode)
            ChangeNodeVisual();
        else
            ChangePartVisual();
    }
    
    private void HandleInitNodeSkillEvent(InitNodeSkillEvent evt)
    {
        PartButton();
    }

    public void PartButton()
    {
        var specialModChangeEvt = SpecialPartNodeEvents.SetSpecialModeEvent;
        specialModChangeEvt.isSpecialMode = false;
        _specialNodeEventChannel.RaiseEvent(specialModChangeEvt);

        ChangePartVisual();
    }

    private void ChangePartVisual()
    {
        _nodeItemView.SetActive(false);
        _partItemView.SetActive(true);
    }

    public void NodeButton()
    {
        var specialModChangeEvt = SpecialPartNodeEvents.SetSpecialModeEvent;
        specialModChangeEvt.isSpecialMode = true;
        _specialNodeEventChannel.RaiseEvent(specialModChangeEvt);

        ChangeNodeVisual();
    }

    private void ChangeNodeVisual()
    {
        _partItemView.SetActive(false);
        _nodeItemView.SetActive(true);

        var evt = NodeChainEvents.ChainModeChangeEvent;
        evt.isActive = false;
        _nodeChainEventChannel.RaiseEvent(evt);
    }
}
