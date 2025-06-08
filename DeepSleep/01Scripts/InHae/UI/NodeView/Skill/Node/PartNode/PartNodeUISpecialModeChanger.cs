using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using YH.EventSystem;

public class PartNodeUISpecialModeChanger : MonoBehaviour, IPartNodeUIComponent
{
    [SerializeField] private GameEventChannelSO _specialPartNodeEventChannel;
    private PartNodeUI _partNodeUI;

    public void Initialize(PartNodeUI partNodeUI)
    {
        _partNodeUI = partNodeUI;
        
        _specialPartNodeEventChannel.AddListener<SetSpecialModeEvent>(HandleSpecialModeChange);
        _specialPartNodeEventChannel.AddListener<ChangeSpecialModeEvent>(HandleChangeSpecialMode);
    }
    
    private void OnDestroy()
    {
        _specialPartNodeEventChannel.RemoveListener<SetSpecialModeEvent>(HandleSpecialModeChange);
        _specialPartNodeEventChannel.RemoveListener<ChangeSpecialModeEvent>(HandleChangeSpecialMode);
    }
    
    private void HandleSpecialModeChange(SetSpecialModeEvent evt)
    {
        _partNodeUI.isSpecialMode = evt.isSpecialMode;
        _partNodeUI.SpecialModeChangedAction?.Invoke(_partNodeUI.isSpecialMode);
    }
    
    private void HandleChangeSpecialMode(ChangeSpecialModeEvent evt)
    {
        _partNodeUI.isSpecialMode = !_partNodeUI.isSpecialMode;
        _partNodeUI.SpecialModeChangedAction?.Invoke(_partNodeUI.isSpecialMode);
    }
}
