using IH.EventSystem.LevelEvent;
using UnityEngine;
using YH.EventSystem;

public class BarrierTrigger : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _startStageEventChannel;
    private BoxCollider _boxCollider;
    
    private LevelRoom _levelRoom;

    private bool _isEnd;

    private void Awake()
    {
        _levelRoom = GetComponentInParent<LevelRoom>();
        _boxCollider = GetComponent<BoxCollider>();
        _startStageEventChannel.AddListener<StageStartEvent>(OnStageStart);
    }

    private void OnDestroy()
    {
        _startStageEventChannel.RemoveListener<StageStartEvent>(OnStageStart);
    }

    private void OnStageStart(StageStartEvent evt)
    {
        if (this.isActiveAndEnabled)
            _boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_levelRoom.isClear)
        {
            var evt = LevelEvents.StageStartEvent;
            _startStageEventChannel.RaiseEvent(evt);
        }
    }
}
