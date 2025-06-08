using IH.EventSystem.NodeEvent.SkillNodeEvents;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;

public class NodeViewUI : MonoBehaviour
{
    [FormerlySerializedAs("_nodeEventChannel")]
    [SerializeField] private GameEventChannelSO _skillNodeEventChannel;
    [SerializeField] private GameEventChannelSO _nodeChainEventChannel;
    
    [SerializeField] private RectTransform _nodeRowParent;
    [SerializeField] private SkillNodeUI _baseSkillNodeUI;
    [SerializeField] private PartNodeUI _partNodeUI;
    
    [SerializeField] private ChainButton _chainButton;
    private bool _isChainMode;
    
    private void Awake()
    {
        _skillNodeEventChannel.AddListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
    }

    private void OnDestroy()
    {
        _skillNodeEventChannel.RemoveListener<InitNodeSkillEvent>(HandleInitNodeSkillEvent);
    }

    private void HandleInitNodeSkillEvent(InitNodeSkillEvent evt)
    {
        for (int i = 0; i < _nodeRowParent.childCount; i++)
            Destroy(_nodeRowParent.GetChild(i).gameObject);
        
        Skill currentSkill = evt.skill;
        SkillInventoryItem currentSkillInventoryItem = evt.skillInventoryItem;

        SkillNodeUI currentSkillNodeUI = Instantiate(_baseSkillNodeUI, _nodeRowParent);
        currentSkillNodeUI.transform.localPosition = Vector3.zero;
        currentSkillNodeUI.Init(currentSkillInventoryItem, currentSkill);
    }
}